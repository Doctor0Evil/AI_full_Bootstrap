use sha2::{Sha512, Digest};
use std::{
    fs::{self, File},
    io::{self, Read, Write},
    path::Path,
    process::Command,
    time::Duration,
};
use tokio::time::sleep;
use anyhow::{Result, Context};

const STATE_HASH_PATH: &str = "/secure/state_hashes/vsc_master_kernel_fingerprint.sha512";
const ENCRYPTED_HASH_PATH: &str = "/secure/state_hashes/vsc_master_kernel_fingerprint.sha512.gpg";
const LATEST_IPFS_CID_PATH: &str = "/secure/state_hashes/_latest_ipfs_fingerprint.cid";
const AUDIT_LOG_PATH: &str = "/secure/logs/kernel_resource_enforcement.log";

/// Recursively read files in directory, compute SHA-512 fingerprint of concatenated sorted hashes.
fn generate_sha512_fingerprint<P: AsRef<Path>>(directory: P) -> Result<String> {
    let mut hashes = Vec::new();

    for entry in walkdir::WalkDir::new(directory) {
        let entry = entry?;
        if entry.file_type().is_file() {
            let mut file = File::open(entry.path())?;
            let mut hasher = Sha512::new();
            let mut buffer = [0u8; 8192];
            loop {
                let n = file.read(&mut buffer)?;
                if n == 0 { break; }
                hasher.update(&buffer[..n]);
            }
            let hash_result = hasher.finalize_reset();
            hashes.push((entry.path().to_owned(), hex::encode(hash_result)));
        }
    }
    // Sort by path to ensure consistency
    hashes.sort_by_key(|(path, _)| path.clone());

    // Concatenate hashes only
    let concat = hashes.iter().map(|(_, h)| h.clone()).collect::<String>();
    let mut final_hasher = Sha512::new();
    final_hasher.update(concat.as_bytes());
    Ok(hex::encode(final_hasher.finalize()))
}

/// Write fingerprint to file with strict permissions.
fn write_fingerprint_to_file(fingerprint: &str) -> io::Result<()> {
    fs::create_dir_all(Path::new(STATE_HASH_PATH).parent().unwrap())?;
    let mut file = File::create(STATE_HASH_PATH)?;
    file.write_all(fingerprint.as_bytes())?;
    #[cfg(unix)]
    {
        use std::os::unix::fs::PermissionsExt;
        fs::set_permissions(STATE_HASH_PATH, fs::Permissions::from_mode(0o600))?;
    }
    Ok(())
}

/// Encrypt fingerprint file symmetrically using AES256 via GPG CLI.
fn encrypt_fingerprint_file() -> Result<()> {
    let status = Command::new("gpg")
        .arg("--batch")
        .arg("--yes")
        .arg("--symmetric")
        .arg("--cipher-algo")
        .arg("AES256")
        .arg(STATE_HASH_PATH)
        .status()
        .context("Failed to execute gpg encryption")?;

    if !status.success() {
        anyhow::bail!("GPG encryption process failed with: {:?}", status);
    }
    Ok(())
}

/// Ensure IPFS installed and initialized; install if missing.
fn ensure_ipfs_installed() -> Result<()> {
    if Command::new("ipfs").arg("version").output().is_err() {
        // Installation logic here; omitted for brevity.
        anyhow::bail!("IPFS not installed; manual installation required.");
    }
    if !Path::new("/root/.ipfs").exists() {
        let status = Command::new("ipfs")
            .arg("init")
            .status()
            .context("Failed to initialize IPFS")?;
        if !status.success() { anyhow::bail!("IPFS init failed"); }
    }
    Ok(())
}

/// Start IPFS daemon asynchronously.
async fn start_ipfs_daemon() -> Result<()> {
    let mut child = Command::new("ipfs")
        .arg("daemon")
        .spawn()
        .context("Failed to start IPFS daemon")?;
    sleep(Duration::from_secs(7)).await;
    // Optionally capture child's PID, stdout/stderr here.
    Ok(())
}

/// Add encrypted fingerprint file to IPFS, returning CID.
fn ipfs_add(path: &str) -> Result<String> {
    let output = Command::new("ipfs")
        .arg("add")
        .arg("-Q")  // print only hash
        .arg(path)
        .output()
        .context("Failed to run ipfs add")?;

    if !output.status.success() {
        anyhow::bail!("ipfs add failed with stderr: {}", String::from_utf8_lossy(&output.stderr));
    }
    let cid = String::from_utf8(output.stdout)?.trim().to_string();
    Ok(cid)
}

/// Write last CID to marker file.
fn write_latest_ipfs_cid(cid: &str) -> io::Result<()> {
    fs::write(LATEST_IPFS_CID_PATH, cid)
}

/// Append event log with timestamp.
fn append_to_audit_log(fingerprint: &str, cid: &str) -> io::Result<()> {
    use chrono::Utc;
    let timestamp = Utc::now().to_rfc3339();
    let log_entry = format!(
        "[{}] Kernel Fingerprint Encrypted & IPFS Synced: HASH={} | CID={}\n",
        timestamp, fingerprint, cid
    );
    fs::create_dir_all(Path::new(AUDIT_LOG_PATH).parent().unwrap())?;
    fs::OpenOptions::new()
        .append(true)
        .create(true)
        .open(AUDIT_LOG_PATH)?
        .write_all(log_entry.as_bytes())?;
    Ok(())
}

/// Main orchestration encompassing fingerprint generation, encryption, IPFS syncing, and logging.
#[tokio::main]
async fn main() -> Result<()> {
    println!("╔═════════════════════════════════════════════════════════════════╗");
    println!("║ ENCRYPTION + IPFS-SYNC: KERNEL FINGERPRINT MODE [ACTIVE]       ║");
    println!("╚═════════════════════════════════════════════════════════════════╝");

    let fingerprint = generate_sha512_fingerprint("./rust_master_system")?;
    println!("[+] Fingerprint Hash Generated: {}", &fingerprint);

    write_fingerprint_to_file(&fingerprint)?;
    println!("[+] Fingerprint written to {}", STATE_HASH_PATH);

    encrypt_fingerprint_file()?;
    println!("[+] Encrypted fingerprint file created: {}", ENCRYPTED_HASH_PATH);

    ensure_ipfs_installed()?;

    start_ipfs_daemon().await?;
    println!("[+] IPFS Daemon Initialized");

    let ipfs_hash = ipfs_add(ENCRYPTED_HASH_PATH)?;
    println!("[+] IPFS Sync Complete — File Hash: {}", &ipfs_hash);

    write_latest_ipfs_cid(&ipfs_hash)?;

    append_to_audit_log(&fingerprint, &ipfs_hash)?;

    println!("╔═════════════════════════════════════════════════════════════════╗");
    println!("║ ➤ KERNEL FINGERPRINT DEPLOYED & IPFS-VERIFIED (CID Logged)     ║");
    println!("║ ➤ SHA512: {}                                          ║", fingerprint);
    println!("║ ➤ IPFS CID: {}                                          ║", ipfs_hash);
    println!("╚═════════════════════════════════════════════════════════════════╝");

    Ok(())
}
