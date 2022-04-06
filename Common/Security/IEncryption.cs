namespace Common.Security
{
    /// <summary>
    /// Defines methods for encryption and decryption.
    /// </summary>
    public interface IEncryption
    {
        /// <summary>
        /// Encrypt text with a provided key and IV.
        /// </summary>
        /// <param name="key">The encryption key.</param>
        /// <param name="initializationVector">The initialization vector.</param>
        /// <param name="clearText">The text to encrypt.</param>
        /// <returns>Encrypted text.</returns>
        string Encrypt(ref string key, ref string initializationVector, string clearText);

        /// <summary>
        /// Encrypt an array of bytes with a provided key and IV.
        /// </summary>
        /// <param name="key">The encryption key.</param>
        /// <param name="initializationVector">The initialization vector.</param>
        /// <param name="textBytes">The array of bytes to encrypt.</param>
        /// <returns>Encrypted text.</returns>
        string Encrypt(ref string key, ref string initializationVector, byte[] textBytes);

        /// <summary>
        /// Decrypt encrypted text with a provided key and IV.
        /// </summary>
        /// <param name="key">The encryption key.</param>
        /// <param name="initializationVector">The initialization vector.</param>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <returns>Decrypted text.</returns>
        string Decrypt(string key, string initializationVector, string encryptedText);

        /// <summary>
        /// Decrypt an array of bytes with a provided key and IV.
        /// </summary>
        /// <param name="key">The encryption key.</param>
        /// <param name="initializationVector">The initialization vector.</param>
        /// <param name="cryptoTextBytes">The array of bytes to decrypt.</param>
        /// <returns>Decrypted text.</returns>
        string Decrypt(string key, string initializationVector, byte[] cryptoTextBytes);
    }
}
