using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Vocare.Util
{
    public static class CriptografiaHelper
    {
        private const string Password = "home5447#m0b1l3";
        private const int SaltSize = 32;

        public static string EncryptString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return null;

            using (var keyDerivation = new Rfc2898DeriveBytes(Password, SaltSize))
            {
                var salt = keyDerivation.Salt;
                var key = keyDerivation.GetBytes(32);
                var iv = keyDerivation.GetBytes(16);

                using (var aesManaged = new AesManaged())
                {
                    aesManaged.KeySize = 256;

                    using (var encryptor = aesManaged.CreateEncryptor(key, iv))
                    {
                        MemoryStream memoryStream = null;

                        try
                        {
                            memoryStream = new MemoryStream();
                            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                            using (var streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }

                            cryptoStream.Dispose();

                            var cipherTextBytes = memoryStream.ToArray();
                            Array.Resize(ref salt, salt.Length + cipherTextBytes.Length);
                            Array.Copy(cipherTextBytes, 0, salt, SaltSize, cipherTextBytes.Length);

                            return Convert.ToBase64String(salt);
                        }
                        finally
                        {
                            if (memoryStream != null)
                            {
                                memoryStream.Dispose();
                            }
                        }
                    }
                }
            }
        }

        public static string DecryptString(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText)) return null;

            var textBytes = Convert.FromBase64String(encryptedText);
            var saltBytes = textBytes.Take(SaltSize).ToArray();
            var encryptedBytes = textBytes.Skip(SaltSize).Take(textBytes.Length - SaltSize).ToArray();

            using (var keyDerivation = new Rfc2898DeriveBytes(Password, saltBytes))
            {
                var key = keyDerivation.GetBytes(32);
                var iv = keyDerivation.GetBytes(16);

                using (var aesManaged = new AesManaged())
                {
                    using (var decryptor = aesManaged.CreateDecryptor(key, iv))
                    {
                        MemoryStream memoryStream = null;

                        try
                        {
                            memoryStream = new MemoryStream(encryptedBytes);
                            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                            var streamReader = new StreamReader(cryptoStream);

                            return streamReader.ReadToEnd();
                        }
                        finally
                        {
                            if (memoryStream != null)
                            {
                                memoryStream.Dispose();
                            }
                        }
                    }
                }
            }
        }

        public static string GenerateHash(string text)
        {
            SHA256 crypto = SHA256.Create();
            var key = Encoding.UTF8.GetBytes(text);

            var hash = crypto.ComputeHash(key);

            return Encoding.UTF8.GetString(hash);
        }

        public static bool IsBase64(this string base64String)
        {
            if (base64String == null || base64String.Length == 0 || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception exception)
            {
                // Handle the exception
            }

            return false;
        }
    }
}
