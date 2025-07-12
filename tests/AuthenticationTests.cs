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
