namespace Common.Security
{
    using System;
    using System.Runtime.Serialization;
    using Resources;

    /// <summary>
    /// Represents an exception that is thrown if a password has expired.
    /// </summary>
    [Serializable]
    public class PasswordExpiredException : ExceptionBase
    {
        private const int ErrorCode = 4011;

        /// <summary>
        /// Initializes a new instance of the Common.Security.PasswordExpiredException class.
        /// </summary>
        public PasswordExpiredException()
            : base(Resource.PasswordExpired, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.PasswordExpiredException class.
        /// </summary>
        /// <param name="message">A message describing why the PasswordExpiredException was thrown.</param>
        public PasswordExpiredException(string message)
            : base(message, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.PasswordExpiredException class.
        /// </summary>
        /// <param name="message">A message describing why the PasswordExpiredException was thrown.</param>
        /// <param name="innerException">The inner exception that caused this PasswordExpiredException to be thrown.</param>
        public PasswordExpiredException(string message, Exception innerException)
            : base(message, ErrorCode, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.PasswordExpiredException class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        protected PasswordExpiredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
