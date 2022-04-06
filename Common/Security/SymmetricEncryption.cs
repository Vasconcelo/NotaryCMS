namespace Common.Security
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using Resources;

    /// <summary>
    /// Provides functionality for symmetric encryption and decryption.
    /// </summary>
    /// <typeparam name="T">The type of encryption to use.</typeparam>
    public abstract class SymmetricEncryption<T> : IEncryption
        where T : SymmetricAlgorithm, new()
    {
        /// <summary>
        /// Encrypt text with a provided key and IV.
        /// </summary>
        /// <param name="key">The encryption key.</param>
        /// <param name="initializationVector">The initialization vector.</param>
        /// <param name="clearText">The text to encrypt.</param>
        /// <returns>Encrypted text.</returns>
        public string Encrypt(ref string key, ref string initializationVector, string clearText)
        {
            if (string.IsNullOrWhiteSpace(clearText))
            {
                throw new ArgumentException(Resource.InputTextRequired);
            }

            var textBytes = Encoding.ASCII.GetBytes(clearText);
            return Encrypt(ref key, ref initializationVector, textBytes);
        }

        /// <summary>
        /// Encrypt an array of bytes with a provided key and IV.
        /// </summary>
        /// <param name="key">The encryption key.</param>
        /// <param name="initializationVector">The initialization vector.</param>
        /// <param name="textBytes">The array of bytes to encrypt.</param>
        /// <returns>Encrypted text.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "This is correct.")]
        public string Encrypt(ref string key, ref string initializationVector, byte[] textBytes)
        {
            if (textBytes == null || textBytes.Length == 0)
            {
                throw new ArgumentException(Resource.InputRequired);
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var crypto = new T())
                {
                    if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(initializationVector))
                    {
                        var keyBytes = Convert.FromBase64String(key);
                        var initializationVectorBytes = Convert.FromBase64String(initializationVector);

                        crypto.Key = keyBytes;
                        crypto.IV = initializationVectorBytes;
                    }

                    using (var cryptTrans = crypto.CreateEncryptor())
                    {
                        using (var cryptStream = new CryptoStream(memoryStream, cryptTrans, CryptoStreamMode.Write))
                        {
                            cryptStream.Write(textBytes, 0, textBytes.Length);
                            cryptStream.FlushFinalBlock();

                            var cryptoTextBytes = memoryStream.ToArray();

                            key = Convert.ToBase64String(crypto.Key);
                            initializationVector = Convert.ToBase64String(crypto.IV);

                            return Convert.ToBase64String(cryptoTextBytes);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decrypt encrypted text with a provided key and IV.
        /// </summary>
        /// <param name="key">The encryption key.</param>
        /// <param name="initializationVector">The initialization vector.</param>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <returns>Decrypted text.</returns>
        public string Decrypt(string key, string initializationVector, string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
            {
                throw new ArgumentException(Resource.EncryptedTextRequired);
            }

            var cryptoTextBytes = Convert.FromBase64String(encryptedText);
            return Decrypt(key, initializationVector, cryptoTextBytes);
        }

        /// <summary>
        /// Decrypt an array of bytes with a provided key and IV.
        /// </summary>
        /// <param name="key">The encryption key.</param>
        /// <param name="initializationVector">The initialization vector.</param>
        /// <param name="cryptoTextBytes">The array of bytes to decrypt.</param>
        /// <returns>Decrypted text.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "This is correct.")]
        public string Decrypt(string key, string initializationVector, byte[] cryptoTextBytes)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException(Resource.EncryptionKeyRequired);
            }

            if (string.IsNullOrWhiteSpace(initializationVector))
            {
                throw new ArgumentException(Resource.IVRequired);
            }

            using (var memoryStream = new MemoryStream())
            {
                var keyBytes = Convert.FromBase64String(key);
                var initializationVectorBytes = Convert.FromBase64String(initializationVector);

                using (var crypto = new T())
                {
                    using (var cryptTrans = crypto.CreateDecryptor(keyBytes, initializationVectorBytes))
                    {
                        using (var cryptStream = new CryptoStream(memoryStream, cryptTrans, CryptoStreamMode.Write))
                        {
                            cryptStream.Write(cryptoTextBytes, 0, cryptoTextBytes.Length);
                            cryptStream.FlushFinalBlock();

                            var textBytes = memoryStream.ToArray();

                            return Encoding.ASCII.GetString(textBytes);
                        }
                    }
                }
            }
        }
    }
}
