namespace Common.Logging
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;

    public class ExceptionLogger : IExceptionLogger
    {
        private readonly ExceptionLoggingOptions _options;

        public ExceptionLogger(IOptions<ExceptionLoggingOptions> options)
        {
            _options = options.Value;
        }

        public void Log(string typeName, string methodName, Guid messageId, Exception e)
        {
            ServiceErrorHelper.LogError(_options.LogStorage, _options.Domain, typeName, methodName, messageId, e);
        }

        public async Task LogAsync(string typeName, string methodName, Guid messageId, Exception e)
        {
            await ServiceErrorHelper.LogErrorAsync(_options.LogStorage, _options.Domain, typeName, methodName, messageId, e);
        }
    }
}
