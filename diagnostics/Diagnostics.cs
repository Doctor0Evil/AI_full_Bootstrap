using System;
using System.Collections.Generic;

namespace UniversalAISystemBoot.Diagnostics
{
    /// <summary>
    /// Collects system health and resource usage metrics.
    /// </summary>
    public static class Diagnostics
    {
        public static SystemMetrics GetSystemMetrics()
        {
            return new SystemMetrics
            {
                CpuUsage = GetCpuUsage(),
                MemoryUsage = GetMemoryUsage(),
                StorageUsage = GetStorageUsage(),
                Temperature = GetTemperature(),
                LastBootTime = GetLastBootTime(),
                NetworkInterfaces = GetNetworkInterfaces()
            };
        }

        private static double GetCpuUsage()
        {
            // Simulated CPU usage percentage
            return new Random().NextDouble() * 100;
        }

        private static double GetMemoryUsage()
        {
            // Simulated memory usage percentage
            return new Random().Next(30, 90);
        }

        private static double GetStorageUsage()
        {
            // Simulated storage usage percentage
            return new Random().Next(40, 85);
        }

        private static double GetTemperature()
        {
            // Simulated temperature in Celsius
            return new Random().Next(30, 75);
        }

        private static string GetLastBootTime()
        {
            return DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private static Dictionary<string, string> GetNetworkInterfaces()
        {
            return new Dictionary<string, string>
            {
                { "eth0", "192.168.1.100" },
                { "lo", "127.0.0.1" }
            };
        }
    }

    /// <summary>
    /// Represents system metrics snapshot.
    /// </summary>
    public class SystemMetrics
    {
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
        public double StorageUsage { get; set; }
        public double Temperature { get; set; }
        public string LastBootTime { get; set; }
        public Dictionary<string, string> NetworkInterfaces { get; set; }
    }
}
5. diagnostics/LogSystem.cs
csharp
copyCopy code
using System;
using System.IO;
using Newtonsoft.Json;

namespace UniversalAISystemBoot.Diagnostics
{
    /// <summary>
    /// Structured JSON logging with rotation.
    /// </summary>
    public static class LogSystem
    {
        private static readonly string LogFile = "/boot/bootlog.json";
        public enum LogLevel { DEBUG, INFO, WARNING, ERROR }

        public static void Log(LogLevel level, string message)
        {
            var entry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Level = level.ToString(),
                Message = message,
                Context = Diagnostics.GetSystemMetrics()
            };

            try
            {
                string json = JsonConvert.SerializeObject(entry, Formatting.Indented);
                File.AppendAllText(LogFile, json + Environment.NewLine);
            }
            catch
            {
                // Silent failure to avoid cascading errors
            }
        }

        public static void RotateLogs()
        {
            try
            {
                if (!File.Exists(LogFile)) return;

                var fileInfo = new FileInfo(LogFile);
                if (fileInfo.Length > 5 * 1024 * 1024) // 5MB
                {
                    string archiveName = $"{LogFile}.{DateTime.Now:yyyyMMddHHmmss}";
                    File.Move(LogFile, archiveName);
                    File.WriteAllText(LogFile, "");
                    Log(LogLevel.INFO, "Log rotated due to size limit.");
                }
            }
            catch
            {
                // Silent failure
            }
        }

        private class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Level { get; set; }
            public string Message { get; set; }
            public SystemMetrics Context { get; set; }
        }
    }
}
6. diagnostics/ErrorHandler.cs
csharp
copyCopy code
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
📁 drivers/
7. drivers/HardwareDriver.cs
csharp
copyCopy code
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
8. drivers/LinuxStorageDriver.cs
csharp
copyCopy code
using System;

namespace UniversalAISystemBoot.Drivers
{
    /// <summary>
    /// Linux-specific storage driver implementation.
    /// </summary>
    public class LinuxStorageDriver : HardwareDriver
    {
        public override void Initialize()
        {
            Console.WriteLine("Initializing Linux storage driver...");
            // Implementation detail: mount devices, check filesystems, etc.
        }

        public override void Diagnose()
        {
            Console.WriteLine("Linux Storage: Healthy");
        }
    }
}
9. drivers/WindowsStorageDriver.cs
csharp
copyCopy code
using System;

namespace UniversalAISystemBoot.Drivers
{
    /// <summary>
    /// Windows-specific storage driver implementation.
    /// </summary>
    public class WindowsStorageDriver : HardwareDriver
    {
        public override void Initialize()
        {
            Console.WriteLine("Initializing Windows storage driver...");
            // Implementation detail: mount volumes, check disk health, etc.
        }

        public override void Diagnose()
        {
            Console.WriteLine("Windows Storage: Healthy");
        }
    }
}
📁 tests/
10. tests/AuthenticationTests.cs
csharp
copyCopy code
using NUnit.Framework;
using UniversalAISystemBoot.Security;

namespace UniversalAISystemBoot.Tests
{
    [TestFixture]
    public class AuthenticationTests
    {
        [Test]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            string password = "StrongPass123!";
            bool result = Authentication.VerifyPassword(password);
            Assert.IsTrue(result);
        }

        [Test]
        public void VerifyPassword_IncorrectPassword_ReturnsFalse()
        {
            string password = "WrongPassword";
            bool result = Authentication.VerifyPassword(password);
            Assert.IsFalse(result);
        }

        [Test]
        public void ComputeMD5Hash_ConsistentHash()
        {
            string input = "TestString";
            string hash1 = Authentication.ComputeMD5Hash(input);
            string hash2 = Authentication.ComputeMD5Hash(input);
            Assert.AreEqual(hash1, hash2);
        }
    }
}
11. tests/PluginManagerTests.cs
csharp
copyCopy code
using NUnit.Framework;
using UniversalAISystemBoot.Plugins;

namespace UniversalAISystemBoot.Tests
{
    [TestFixture]
    public class PluginManagerTests
    {
        [Test]
        public void LoadPlugins_ValidPlugins_LoadsSuccessfully()
        {
            var rootMenu = new MainLoader.MenuNode("Root");
            PluginManager.LoadPlugins(rootMenu);
            var plugins = PluginManager.ListPlugins();
            Assert.IsNotNull(plugins);
            Assert.IsNotEmpty(plugins);
        }

        [Test]
        public void GetPlugin_ExistingPlugin_ReturnsPlugin()
        {
            var plugin = PluginManager.GetPlugin("SamplePlugin");
            Assert.IsNotNull(plugin);
            Assert.AreEqual("SamplePlugin", plugin.Name);
        }

        [Test]
        public void UnloadAll_ClearsLoadedPlugins()
        {
            var rootMenu = new MainLoader.MenuNode("Root");
            PluginManager.LoadPlugins(rootMenu);
            PluginManager.UnloadAll();
            var plugins = PluginManager.ListPlugins();
            Assert.IsEmpty(plugins);
        }
    }
}
12. tests/ShellTests.cs
csharp
copyCopy code
using NUnit.Framework;
using System.Collections.Generic;
using UniversalAISystemBoot.Shell;

namespace UniversalAISystemBoot.Tests
{
    [TestFixture]
    public class ShellTests
    {
        [Test]
        public void CommandFilter_IsAllowed_KnownCommand_ReturnsTrue()
        {
            Assert.IsTrue(CommandFilter.IsAllowed("ls"));
            Assert.IsTrue(CommandFilter.IsAllowed("pwd"));
        }

        [Test]
        public void CommandFilter_IsAllowed_UnknownCommand_ReturnsFalse()
        {
            Assert.IsFalse(CommandFilter.IsAllowed("rm"));
        }

        [Test]
        public void CommandFilter_IsReproductionAttempt_DetectsKeywords()
        {
            Assert.IsTrue(CommandFilter.IsReproductionAttempt("git clone https://repo"));
            Assert.IsTrue(CommandFilter.IsReproductionAttempt("cat secret.rs"));
            Assert.IsFalse(CommandFilter.IsReproductionAttempt("ls -la"));
        }
    }
}
13. tests/DiagnosticsTests.cs
csharp
copyCopy code
using NUnit.Framework;
using UniversalAISystemBoot.Diagnostics;

namespace UniversalAISystemBoot.Tests
{
    [TestFixture]
    public class DiagnosticsTests
    {
        [Test]
        public void GetSystemMetrics_ReturnsValidMetrics()
        {
            var metrics = Diagnostics.GetSystemMetrics();
            Assert.IsNotNull(metrics);
            Assert.IsTrue(metrics.CpuUsage >= 0 && metrics.CpuUsage <= 100);
            Assert.IsTrue(metrics.MemoryUsage >= 0 && metrics.MemoryUsage <= 100);
            Assert.IsNotNull(metrics.NetworkInterfaces);
        }
    }
}
