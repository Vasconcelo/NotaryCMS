namespace Common
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ExceptionBase : Exception
    {
        private const int DefaultCode = 5000;
        private readonly int _code;

        public ExceptionBase()
            : base("An exception occurred.")
        {
            _code = DefaultCode;
        }

        public ExceptionBase(string message)
            : base(message)
        {
            _code = DefaultCode;
        }

        public ExceptionBase(string message, int code)
            : base(message)
        {
            _code = code;
        }

        public ExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
            _code = DefaultCode;
        }

        public ExceptionBase(string message, int code, Exception innerException)
            : base(message, innerException)
        {
            _code = code;
        }

        protected ExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _code = info.GetInt32("_code");
        }

        public int Code
        {
            get
            {
                return _code;
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("_code", _code);
            base.GetObjectData(info, context);
        }
    }
}
