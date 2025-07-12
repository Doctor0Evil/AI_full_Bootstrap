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
