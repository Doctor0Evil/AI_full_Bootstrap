use crate::schema::FileSystemConfig;
pub fn mount(fs: &FileSystemConfig) -> Result<(), String> {
    println!("💽 Mounting {} with {}", fs.mount_at, fs.encryption.algorithm);
    if fs.encryption.quantum_resistant {
        println!("   🔐 PQ Crypto enabled");
    }
    Ok(())
}
