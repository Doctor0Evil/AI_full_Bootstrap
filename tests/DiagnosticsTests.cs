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
