namespace Common.Data
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DataNotFoundException : ExceptionBase
    {
        public DataNotFoundException()
            : base("Data not found.", 4004)
        {
        }

        public DataNotFoundException(string message)
            : base(message, 4004)
        {
        }

        public DataNotFoundException(string message, Exception innerException)
            : base(message, 4004, innerException)
        {
        }

        protected DataNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
