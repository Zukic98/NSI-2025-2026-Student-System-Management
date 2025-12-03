using System.Security.Cryptography;
using System.Text;
using Identity.Application.Interfaces;

namespace Identity.Infrastructure.Services
{
    public class SecretEncryptionService : ISecretEncryptionService
    {
        // DEV KEY – samo za lokalni razvoj, kasnije iz configa / KeyVault-a
        private static readonly byte[] Key;

        static SecretEncryptionService()
        {
            const string keyString = "12345678901234567890123456789012"; // 32 chars = 256-bit

            if (keyString.Length != 32)
                throw new InvalidOperationException("Encryption key must be 32 characters long.");

            Key = Encoding.UTF8.GetBytes(keyString);
        }

        public string Encrypt(string plaintext)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.GenerateIV(); // random IV za svaki poziv

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plaintext);
            var encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // [IV][CIPHER] → base64
            var combined = aes.IV.Concat(encrypted).ToArray();
            return Convert.ToBase64String(combined);
        }

        public string Decrypt(string ciphertext)
        {
            var bytes = Convert.FromBase64String(ciphertext);

            var iv = bytes.Take(16).ToArray();        // prvih 16 bajtova je IV
            var cipher = bytes.Skip(16).ToArray();    // ostatak je cipher

            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            var decrypted = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
