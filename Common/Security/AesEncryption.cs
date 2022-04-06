namespace Common.Security
{
    using System.Security.Cryptography;

    /// <summary>
    /// Provides functionality for symmetric encryption and decryption using the Advanced Encryption Standard (AES) algorithm.
    /// </summary>
    /// <remarks>This class is PCI compliant.</remarks>
    public sealed class AesEncryption : SymmetricEncryption<AesCryptoServiceProvider>
    {
    }
}
