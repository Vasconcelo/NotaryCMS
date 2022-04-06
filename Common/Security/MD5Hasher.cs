namespace Common.Security
{
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Provides functionality for MD5 hashing.
    /// </summary>
    public class MD5Hasher : IHasher
    {
        /// <summary>
        /// Hash text using the MD5 hashing algorithm.
        /// </summary>
        /// <param name="input">The text to hash.</param>
        /// <returns>Hashed text.</returns>
        public string Hash(string input)
        {
            var encoding = new UTF8Encoding();
            var textAsBytes = encoding.GetBytes(input);

            using (var md5 = new MD5CryptoServiceProvider())
            {
                var hash = md5.ComputeHash(textAsBytes);
                return HashToString(hash);
            }
        }

        private static string HashToString(byte[] hash)
        {
            var result = new StringBuilder();
            foreach (var position in hash)
            {
                result.Append(position.ToString("x2", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture));
            }

            return result.ToString();
        }
    }
}
