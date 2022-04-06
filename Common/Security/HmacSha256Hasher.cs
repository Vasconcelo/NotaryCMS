namespace Common.Security
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Provides functionality for HMAC SHA256 hashing.
    /// </summary>
    public class HmacSha256Hasher : IHmacSha256Hasher
    {
        /// <summary>
        /// Hash text using the HMAC SHA256 hashing algorithm.
        /// </summary>
        /// <param name="input">The text to hash.</param>
        /// <param name="key">The key.</param>
        /// <returns>Hashed text.</returns>
        public string Hash(string input, string key)
        {
            var textAsBytes = Encoding.UTF8.GetBytes(input);
            var keyAsBytes = Convert.FromBase64String(key);

            using (var mac = new HMACSHA256(keyAsBytes))
            {
                var hash = mac.ComputeHash(textAsBytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
