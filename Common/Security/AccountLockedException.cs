namespace Common.Security
{
    using System;
    using System.Runtime.Serialization;
    using Resources;

    /// <summary>
    /// Represents an exception that is thrown when a user account was locked, most likely from too many login retries.
    /// </summary>
    [Serializable]
    public class AccountLockedException : ExceptionBase
    {
        private const int ErrorCode = 4012;

        /// <summary>
        /// Initializes a new instance of the Common.Security.AccountLockedException class.
        /// </summary>
        public AccountLockedException()
            : base(Resource.AccountLocked, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.AccountLockedException class.
        /// </summary>
        /// <param name="message">A message describing why the AccountLockedException was thrown.</param>
        public AccountLockedException(string message)
            : base(message, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.AccountLockedException class.
        /// </summary>
        /// <param name="message">A message describing why the AccountLockedException was thrown.</param>
        /// <param name="innerException">The inner exception that caused this AccountLockedException to be thrown.</param>
        public AccountLockedException(string message, Exception innerException)
            : base(message, ErrorCode, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.AccountLockedException class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        protected AccountLockedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
