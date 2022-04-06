namespace Common.Security
{
    using System;
    using System.Runtime.Serialization;
    using Resources;

    /// <summary>
    /// Represents an exception that is thrown when a user account was locked by an administrator.
    /// </summary>
    [Serializable]
    public class AccountLockedByAdminException : ExceptionBase
    {
        private const int ErrorCode = 4013;

        /// <summary>
        /// Initializes a new instance of the Common.Security.AccountLockedByAdminException class.
        /// </summary>
        public AccountLockedByAdminException()
            : base(Resource.AccountLockedByAdmin, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.AccountLockedByAdminException class.
        /// </summary>
        /// <param name="message">A message describing why the AccountLockedByAdminException was thrown.</param>
        public AccountLockedByAdminException(string message)
            : base(message, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.AccountLockedByAdminException class.
        /// </summary>
        /// <param name="message">A message describing why the AccountLockedByAdminException was thrown.</param>
        /// <param name="innerException">The inner exception that caused this AccountLockedByAdminException to be thrown.</param>
        public AccountLockedByAdminException(string message, Exception innerException)
            : base(message, ErrorCode, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.AccountLockedByAdminException class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        protected AccountLockedByAdminException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
