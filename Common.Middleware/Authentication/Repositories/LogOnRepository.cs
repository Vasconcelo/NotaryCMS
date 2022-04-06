namespace Common.Middleware.Authentication.Repositories
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public class LogOnRepository : ILogOnRepository
    {
        private const string TableName = "LogOn";
        private readonly CloudTable _table;

        public LogOnRepository(IOptions<AuthenticationOptions> options)
        {
            var storageAccount = CloudStorageAccount.Parse(options.Value.OpsStorage);
            var tableClient = storageAccount.CreateCloudTableClient();

            _table = tableClient.GetTableReference(TableName);
            _table.CreateIfNotExists();
        }

        public async Task<LogOnEntity> RetrieveAsync(string partitionKey, string rowKey)
        {
            try
            {
                var retrieveOperation = TableOperation.Retrieve<LogOnEntity>(partitionKey.ToLowerInvariant(), rowKey);
                var retrievedResult = await _table.ExecuteAsync(retrieveOperation);

                if (retrievedResult.Result == null)
                {
                    return null;
                }

                return (LogOnEntity)retrievedResult.Result;
            }
            catch
            {
                return null;
            }
        }
    }
}
