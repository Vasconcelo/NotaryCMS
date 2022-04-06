namespace Common.Security
{
    using System;
    using System.Runtime.Serialization;
    using Resources;

    /// <summary>
    /// Represents an exception that is thrown if a passcode has expired.
    /// </summary>
    [Serializable]
    public class PasscodeExpiredException : ExceptionBase
    {
        private const int ErrorCode = 4016;

        /// <summary>
        /// Initializes a new instance of the Common.Security.PasscodeExpiredException class.
        /// </summary>
        public PasscodeExpiredException()
            : base(Resource.PasscodeExpired, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.PasscodeExpiredException class.
        /// </summary>
        /// <param name="message">A message describing why the PasscodeExpiredException was thrown.</param>
        public PasscodeExpiredException(string message)
            : base(message, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.PasscodeExpiredException class.
        /// </summary>
        /// <param name="message">A message describing why the PasscodeExpiredException was thrown.</param>
        /// <param name="innerException">The inner exception that caused this PasscodeExpiredException to be thrown.</param>
        public PasscodeExpiredException(string message, Exception innerException)
            : base(message, ErrorCode, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.PasscodeExpiredException class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        protected PasscodeExpiredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
