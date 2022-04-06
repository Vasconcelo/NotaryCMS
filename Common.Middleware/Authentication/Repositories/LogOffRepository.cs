namespace Common.Middleware.Authentication.Repositories
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public class LogOffRepository : ILogOffRepository
    {
        private const string TableName = "LogOff";
        private readonly CloudTable _table;

        public LogOffRepository(IOptions<AuthenticationOptions> options)
        {
            var storageAccount = CloudStorageAccount.Parse(options.Value.OpsStorage);
            var tableClient = storageAccount.CreateCloudTableClient();

            _table = tableClient.GetTableReference(TableName);
            _table.CreateIfNotExists();
        }

        public async Task<LogOffEntity> RetrieveAsync(string partitionKey, string rowKey)
        {
            try
            {
                var retrieveOperation = TableOperation.Retrieve<LogOffEntity>(partitionKey.ToLowerInvariant(), rowKey);
                var retrievedResult = await _table.ExecuteAsync(retrieveOperation);

                if (retrievedResult.Result == null)
                {
                    return null;
                }

                return (LogOffEntity)retrievedResult.Result;
            }
            catch
            {
                return null;
            }
        }
    }
}
