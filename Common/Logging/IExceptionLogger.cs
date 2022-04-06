namespace Common.Logging
{
    using System;
    using System.Threading.Tasks;

    public interface IExceptionLogger
    {
        void Log(
            string typeName,
            string methodName,
            Guid messageId,
            Exception e);

        Task LogAsync(
            string typeName,
            string methodName,
            Guid messageId,
            Exception e);
    }
}
