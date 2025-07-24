use crate::schema::ComplianceConfig;
pub fn apply(cfg: &ComplianceConfig) -> Result<(), String> {
    println!("⚖️ Compliance:");
    if cfg.gdpr { println!("   ✅ GDPR"); }
    if cfg.ccpa { println!("   ✅ CCPA"); }
    if cfg.audit_log { println!("   📜 Auditing"); }
    Ok(())
}
