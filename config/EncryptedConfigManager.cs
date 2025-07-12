using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace UniversalAISystemBoot.Config
{
    /// <summary>
    /// Manages encrypted configuration loading and saving.
    /// </summary>
    public static class EncryptedConfigManager
    {
        private const string ConfigFilePath = "/boot/config.enc";
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("16ByteSecretKey!"); // AES-128 key (example)
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("16ByteInitVector");   // Initialization vector

        private static dynamic ConfigData;

        public static void LoadConfig()
        {
            if (!File.Exists(ConfigFilePath))
            {
                ConfigData = new { };
                return;
            }

            byte[] encryptedData = File.ReadAllBytes(ConfigFilePath);
            string json = Decrypt(encryptedData);
            ConfigData = JsonConvert.DeserializeObject(json);
        }

        public static void SaveConfig()
        {
            string json = JsonConvert.SerializeObject(ConfigData, Formatting.Indented);
            byte[] encryptedData = Encrypt(json);
            File.WriteAllBytes(ConfigFilePath, encryptedData);
        }

        public static T Get<T>(string key, T defaultValue = default)
        {
            if (ConfigData == null) LoadConfig();

            try
            {
                var val = ConfigData[key];
                return val == null ? defaultValue : (T)Convert.ChangeType(val, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static void Set(string key, object value)
        {
            if (ConfigData == null) LoadConfig();
            ConfigData[key] = value;
            SaveConfig();
        }

        private static byte[] Encrypt(string plainText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
            return ms.ToArray();
        }

        private static string Decrypt(byte[] cipherText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(cipherText);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
