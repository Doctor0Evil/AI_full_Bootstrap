use crate::schema::AIModelDescriptor;
pub fn initialize(models: &[AIModelDescriptor]) -> Result<(), String> {
    for model in models {
        println!("🧠 AI: {} {} S{} {}", model.name, model.version,
                 model.security_level, if model.isolated {"[Isolated]"} else {"[Shared]"} );
    }
    Ok(())
}
