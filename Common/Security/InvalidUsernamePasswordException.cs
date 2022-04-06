namespace Common.Security
{
    using System;
    using System.Runtime.Serialization;
    using Resources;

    /// <summary>
    /// Represents an exception that is thrown when a username / password pair doesn't exist.
    /// </summary>
    [Serializable]
    public class InvalidUsernamePasswordException : ExceptionBase
    {
        private const int ErrorCode = 4010;

        /// <summary>
        /// Initializes a new instance of the Common.Security.InvalidUsernamePasswordException class.
        /// </summary>
        public InvalidUsernamePasswordException()
            : base(Resource.InvalidUsernamePassword, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.InvalidUsernamePasswordException class.
        /// </summary>
        /// <param name="message">A message describing why the InvalidUsernamePasswordException was thrown.</param>
        public InvalidUsernamePasswordException(string message)
            : base(message, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.InvalidUsernamePasswordException class.
        /// </summary>
        /// <param name="message">A message describing why the InvalidUsernamePasswordException was thrown.</param>
        /// <param name="innerException">The inner exception that caused this InvalidUsernamePasswordException to be thrown.</param>
        public InvalidUsernamePasswordException(string message, Exception innerException)
            : base(message, ErrorCode, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Security.InvalidUsernamePasswordException class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        protected InvalidUsernamePasswordException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
