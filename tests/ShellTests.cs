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
