using System;
using System.Threading;

namespace UniversalAISystemBoot.Diagnostics
{
    /// <summary>
    /// Handles unhandled exceptions and system health checks.
    /// </summary>
    public static class ErrorHandler
    {
        public static void HandleException(Exception ex)
        {
            LogSystem.Log(LogSystem.LogLevel.ERROR, $"Unhandled exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
            if (ConfigManager.Get<bool>("AutoRebootOnCrash"))
            {
                Console.WriteLine("System crash detected. Rebooting in 10 seconds...");
                Thread.Sleep(10000);
                SystemControl.Reboot();
            }
            else
            {
                Console.WriteLine("System halted due to critical error.");
                SystemControl.Halt();
            }
        }

        public static void CheckSystemHealth()
        {
            var metrics = Diagnostics.GetSystemMetrics();
            if (metrics.CpuUsage > 95 || metrics.MemoryUsage > 95)
            {
                LogSystem.Log(LogSystem.LogLevel.WARNING, "High resource usage detected. Initiating cleanup...");
                // Trigger garbage collection or process termination as needed
            }
        }
    }
}
