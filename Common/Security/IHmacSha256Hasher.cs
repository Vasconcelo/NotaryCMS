namespace Common.Security
{
    public interface IHmacSha256Hasher
    {
        /// <summary>
        /// Hash text.
        /// </summary>
        /// <param name="input">The text to hash.</param>
        /// <param name="key">The key.</param>
        /// <returns>Hashed text.</returns>
        string Hash(string input, string key);
    }
}
