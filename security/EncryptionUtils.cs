using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UniversalAISystemBoot.Security
{
    /// <summary>
    /// Helper methods for cryptographic operations.
    /// </summary>
    public static class EncryptionUtils
    {
        /// <summary>
        /// Computes SHA256 hash of a file.
        /// </summary>
        public static string ComputeSHA256Hash(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Signs data using RSA private key.
        /// </summary>
        public static byte[] RSASignData(byte[] data, RSAParameters privateKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportParameters(privateKey);
            return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        /// <summary>
        /// Verifies RSA signature.
        /// </summary>
        public static bool RSAVerifySignature(byte[] data, byte[] signature, RSAParameters publicKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportParameters(publicKey);
            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
