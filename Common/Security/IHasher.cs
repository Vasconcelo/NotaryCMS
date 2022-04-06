namespace Common.Security
{
    /// <summary>
    /// Defines methods for hashing.
    /// </summary>
    public interface IHasher
    {
        /// <summary>
        /// Hash text.
        /// </summary>
        /// <param name="input">The text to hash.</param>
        /// <returns>Hashed text.</returns>
        string Hash(string input);
    }
}
