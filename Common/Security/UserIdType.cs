namespace Common.Security
{
    /// <summary>
    /// Provides an enumerated list of user id types.
    /// </summary>
    public enum UserIdType
    {
        /// <summary>
        /// The user id type is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The user type is an api key.
        /// </summary>
        ApiKey,

        /// <summary>
        /// The user type is OAuth.
        /// </summary>
        Federation
    }
}
