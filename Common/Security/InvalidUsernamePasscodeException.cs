namespace Common.Security
{
    using System;
    using System.Runtime.Serialization;
    using Resources;

    /// <summary>
    /// Represents an exception that is thrown when a username / passcode pair doesn't exist.
    /// </summary>
    [Serializable]
    public class InvalidUsernamePasscodeException : ExceptionBase
    {
        private const int ErrorCode = 4015;

        /// <summary>
        /// Initializes a new instance of the Common.Security.InvalidUsernamePasscodeException class.
        /// </summary>
        public InvalidUsernamePasscodeException()
            : base(Resource.InvalidUsernamePasscode, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.InvalidUsernamePasscodeException class.
        /// </summary>
        /// <param name="message">A message describing why the InvalidUsernamePasscodeException was thrown.</param>
        public InvalidUsernamePasscodeException(string message)
            : base(message, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.InvalidUsernamePasscodeException class.
        /// </summary>
        /// <param name="message">A message describing why the InvalidUsernamePasscodeException was thrown.</param>
        /// <param name="innerException">The inner exception that caused this InvalidUsernamePasscodeException to be thrown.</param>
        public InvalidUsernamePasscodeException(string message, Exception innerException)
            : base(message, ErrorCode, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.InvalidUsernamePasscodeException class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        protected InvalidUsernamePasscodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
