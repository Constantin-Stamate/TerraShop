using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace eUseControl.Helpers
{
    public static class CookieGenerator
    {
        private const string SaltData = "QADLz4qk3rVgBSGjDfAH3XWV" + "qKKagMXezBPv7TmXvwnXDDeR" + "pHaLBv4JnTGRwLg9tzbmV77g" + "8DUEAEa6JPv66hy7SwHBL4z4" + "FbGdh2MVs4kq9RcaZEAszuP5"
                                        + "ccLsEfqCpwdSvVVt479DCZrw" + "jSHrJVwaja9WQaWAmEY9NsPv" + "EHKnFwHTGAvPXpjpCxkbedYq" + "uEauLvZLphwmJLUteZ4QAXU6" + "Z4F3PDmh3wsQXvSctQBHvNWf";
        private static readonly byte[] Salt = Encoding.ASCII.GetBytes(SaltData);

        public static string Create(string value)
        {
            return EncryptStringAes(value, "BjXNmq5MKKaraLwxz9uaATvFwE4Rj679KguTRE8c2j56FnkuKJKfkGbZEeDGFDvsGYNHpUXFUUUuUHBR4UV3T2kumguhubg6Gpt7CyqGDbUPrMvPc67kX3yP");
        }

        public static string Validate(string value)
        {
            return DecryptStringAes(value, "BjXNmq5MKKaraLwxz9uaATvFwE4Rj679KguTRE8c2j56FnkuKJKfkGbZEeDGFDvsGYNHpUXFUUUuUHBR4UV3T2kumguhubg6Gpt7CyqGDbUPrMvPc67kX3yP");
        }

        private static string EncryptStringAes(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException(nameof(sharedSecret));

            string outStr;
            RijndaelManaged aesAlg = null;

            try
            {
                var key = new Rfc2898DeriveBytes(sharedSecret, Salt);

                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                aesAlg?.Clear();
            }

            return outStr;
        }

        private static string DecryptStringAes(string cipherText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(nameof(cipherText));
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException(nameof(sharedSecret));

            string plainText;
            RijndaelManaged aesAlg = null;

            try
            {
                var key = new Rfc2898DeriveBytes(sharedSecret, Salt);

                var bytes = Convert.FromBase64String(cipherText);
                using (var msDecrypt = new MemoryStream(bytes))
                {
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                    aesAlg.IV = ReadByteArray(msDecrypt);

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                            plainText = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plainText;
        }

        private static byte[] ReadByteArray(Stream s)
        {
            var rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
                throw new SystemException("Stream did not contain properly formatted byte array");

            var buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
                throw new SystemException("Did not read byte array properly");

            return buffer;
        }
    }
}
