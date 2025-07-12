namespace UniversalAISystemBoot.MinimalLoader
{
    class MinimalLoader
    {
        static void Main()
        {
            Platform.Detect();
            Hardware.Init();
            Display.Banner("=== UNIVERSAL AI SYSTEM BOOT v1.0 ===");

            if (!Security.VerifyBootSignature())
            {
                Display.Error("Integrity check failed. System halted.");
                SystemControl.Halt();
            }

            Capability.Adapt();
            IntermediateLoader.Launch();
        }
    }
}
