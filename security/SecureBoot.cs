using System;
using System.IO;

namespace UniversalAISystemBoot.Security
{
    /// <summary>
    /// Secure boot enforcement with cryptographic verification.
    /// </summary>
    public static class SecureBoot
    {
        private const string BootloaderPath = "/boot/loader.bin";
        private const string ExpectedHash = "a1b2c3d4e5f67890abcdef1234567890abcdef12"; // Example

        /// <summary>
        /// Verifies bootloader integrity.
        /// </summary>
        public static bool VerifyBootSignature()
        {
            try
            {
                if (!File.Exists(BootloaderPath))
                {
                    Console.WriteLine("Bootloader binary not found.");
                    return false;
                }

                string actualHash = EncryptionUtils.ComputeSHA256Hash(BootloaderPath);
                bool verified = string.Equals(actualHash, ExpectedHash, StringComparison.OrdinalIgnoreCase);

                if (!verified)
                    Console.WriteLine("Boot signature mismatch.");

                return verified;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Secure boot verification error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Enforces secure boot; halts system if verification fails.
        /// </summary>
        public static void EnforceSecureBoot()
        {
            if (!VerifyBootSignature())
            {
                Console.WriteLine("Secure Boot verification failed. Halting system.");
                SystemControl.Halt();
            }
            else
            {
                Console.WriteLine("Secure Boot verified successfully.");
            }
        }
    }
}
