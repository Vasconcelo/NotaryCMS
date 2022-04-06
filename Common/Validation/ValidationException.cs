namespace Common.Validation
{
    using System;
    using System.Runtime.Serialization;
    using Resources;

    /// <summary>
    /// Represents exception that is thrown when data is not valid.
    /// </summary>
    [Serializable]
    public class ValidationException : ExceptionBase
    {
        private const int ErrorCode = 4002;

        /// <summary>
        /// Initializes a new instance of the Common.Validation.ValidationException class.
        /// </summary>
        public ValidationException()
            : base(Resource.InvalidData, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Validation.ValidationException class.
        /// </summary>
        /// <param name="message">A message describing why the ValidationException was thrown.</param>
        public ValidationException(string message)
            : base(message, ErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Validation.ValidationException class.
        /// </summary>
        /// <param name="message">A message describing why the ValidationException was thrown.</param>
        /// <param name="innerException">The inner exception that caused this ValidationException to be thrown.</param>
        public ValidationException(string message, Exception innerException)
            : base(message, ErrorCode, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Common.Validation.ValidationException class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
