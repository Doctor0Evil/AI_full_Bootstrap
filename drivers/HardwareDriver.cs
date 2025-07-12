namespace UniversalAISystemBoot.Drivers
{
    /// <summary>
    /// Abstract base class for hardware drivers.
    /// </summary>
    public abstract class HardwareDriver
    {
        /// <summary>
        /// Initialize the driver and hardware components.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Run diagnostics on the hardware device.
        /// </summary>
        public abstract void Diagnose();
    }
}
