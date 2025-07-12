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
