namespace Common.Logging
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides helper functionality to log exceptions to Azure Table Storage.
    /// </summary>
    public static class ServiceErrorHelper
    {
        private const string TableName = "ServiceError";

        /// <summary>
        /// Save an exception to Azure Table Storage.
        /// </summary>
        /// <param name="connectionString">The storage connection string.</param>
        /// <param name="domain">The domain where the error originated.</param>
        /// <param name="typeName">The type name of the class where the error originated.</param>
        /// <param name="methodName">The method name where the error originated.</param>
        /// <param name="messageId">The id of the message.</param>
        /// <param name="e">The exception to log.</param>
        public static void LogError(
            string connectionString,
            string domain,
            string typeName,
            string methodName,
            Guid messageId,
            Exception e)
        {
            dynamic entity = new ExpandoObject();
            entity.MethodName = methodName;

            var i = 0;
            var entityDictionary = entity as IDictionary<string, object>;
            foreach (var datum in e.Data)
            {
                if (i > 3)
                {
                    break;
                }

                i++;
                var entry = (DictionaryEntry)datum;

                if (entry.Key == null)
                {
                    continue;
                }

                var key = entry.Key
                    .ToString()
                    .Replace(" ", string.Empty)
                    .Replace("@", string.Empty)
                    .Replace(":", string.Empty)
                    .Replace("<", string.Empty)
                    .Replace(">", string.Empty)
                    .Replace(".", string.Empty);

                double keyNum;
                if (double.TryParse(key, out keyNum))
                {
                    key = string.Format(CultureInfo.InvariantCulture, "e{0}", key);
                }

                entityDictionary.Add(new KeyValuePair<string, object>(key, entry.Value.ToString()));
            }

            entity.ErrorCode = GetErrorCode(e);
            entity.ErrorMessage = e.Message;
            entity.MessageId = messageId;
            AddInnerMessage(entity, e.InnerException, 1);
            entity.StackTrace = e.StackTrace.PadLeft(2048);

            var partitionKey = string.Format(
                CultureInfo.InvariantCulture,
                "{0}_{1}.{2}",
                domain,
                typeName,
                methodName);

            var rowKey = (DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks)
                .ToString(CultureInfo.InvariantCulture)
                .PadLeft(19, '0');

            var table = new ExpandoCloudTable(connectionString, TableName);
            table.SaveEntity(entity, partitionKey, rowKey);
        }

        /// <summary>
        /// Save an exception to Azure Table Storage asynchronously.
        /// </summary>
        /// <param name="connectionString">The storage connection string.</param>
        /// <param name="domain">The domain where the error originated.</param>
        /// <param name="typeName">The type name of the class where the error originated.</param>
        /// <param name="methodName">The method name where the error originated.</param>
        /// <param name="messageId">The id of the message.</param>
        /// <param name="e">The exception to log.</param>
        /// <returns>A Task that logs the error asynchronously.</returns>
        public static async Task LogErrorAsync(
            string connectionString,
            string domain,
            string typeName,
            string methodName,
            Guid messageId,
            Exception e)
        {
            dynamic entity = new ExpandoObject();
            entity.MethodName = methodName;

            var i = 0;
            var entityDictionary = entity as IDictionary<string, object>;
            foreach (var datum in e.Data)
            {
                if (i > 3)
                {
                    break;
                }

                i++;
                var entry = (DictionaryEntry)datum;

                if (entry.Key == null)
                {
                    continue;
                }

                var key = entry.Key
                    .ToString()
                    .Replace(" ", string.Empty)
                    .Replace("@", string.Empty)
                    .Replace(":", string.Empty)
                    .Replace("<", string.Empty)
                    .Replace(">", string.Empty)
                    .Replace(".", string.Empty);

                double keyNum;
                if (double.TryParse(key, out keyNum))
                {
                    key = string.Format(CultureInfo.InvariantCulture, "e{0}", key);
                }

                entityDictionary.Add(new KeyValuePair<string, object>(key, entry.Value.ToString()));
            }

            entity.ErrorCode = GetErrorCode(e);
            entity.ErrorMessage = e.Message;
            entity.MessageId = messageId;
            AddInnerMessage(entity, e.InnerException, 1);
            entity.StackTrace = e.StackTrace.PadLeft(2048);

            var partitionKey = string.Format(
                CultureInfo.InvariantCulture,
                "{0}_{1}.{2}",
                domain,
                typeName,
                methodName);

            var rowKey = (DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks)
                .ToString(CultureInfo.InvariantCulture)
                .PadLeft(19, '0');

            var table = new ExpandoCloudTable(connectionString, TableName);
            await table.SaveEntityAsync(entity, partitionKey, rowKey);
        }

        /// <summary>
        /// Gets an error code from the exception.
        /// </summary>
        /// <param name="exceptionToLog">
        /// The exception to log.
        /// </param>
        /// <returns>
        /// The error code.
        /// </returns>
        public static int GetErrorCode(Exception exceptionToLog)
        {
            var exceptionBaseToLog = exceptionToLog as ExceptionBase;
            if (exceptionBaseToLog != null && exceptionBaseToLog.Code != 0)
            {
                return exceptionBaseToLog.Code;
            }

            return -1;
        }

        private static void AddInnerMessage(dynamic entity, Exception e, int level)
        {
            if (level > 3)
            {
                return;
            }

            if (e != null && e.InnerException != null)
            {
                var entityAttributes = (IDictionary<string, object>)entity;

                entityAttributes.Add(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "InnerMessage{0}",
                        level.ToString(CultureInfo.InvariantCulture)),
                        e.InnerException.Message.PadLeft(255));

                level++;
                AddInnerMessage(entity, e.InnerException, level);
            }
        }
    }
}
