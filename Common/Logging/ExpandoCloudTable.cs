namespace Common.Logging
{
    using System.Dynamic;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Provides functionality to store ExpandoObject entities in Azure Table Storage.
    /// </summary>
    public class ExpandoCloudTable : CloudTableBase<ExpandoObject>
    {
        /// <summary>
        /// Initializes a new instance of the ExpandoCloudTable class.
        /// </summary>
        /// <param name="connectionString">A connection string for the storage account.</param>
        /// <param name="tableName">The name of the table that will store the ExpandoObject entities.</param>
        public ExpandoCloudTable(string connectionString, string tableName)
            : base(connectionString, tableName)
        {
        }

        /// <summary>
        /// Save a ExpandoObject entity to Azure Table Storage.
        /// </summary>
        /// <param name="entity">The ExpandoObject entity to save.</param>
        /// <param name="partitionKey">The Azure Table Storage partition key.</param>
        /// <param name="rowKey">The Azure Table Storage row key.</param>
        /// <remarks>
        /// ExpandoObject values must be of the following types: float, decimal, double, int, short, byte, string, bool, DateTime, Guid, or Uri. 
        /// All other types will be ignored.
        /// </remarks>
        public override void SaveEntity(ExpandoObject entity, string partitionKey, string rowKey)
        {
            Table.CreateIfNotExistsAsync();

            var dynamicTableEntity = entity.ToDynamicTableEntity(partitionKey, rowKey);
            Table.ExecuteAsync(TableOperation.InsertOrReplace(dynamicTableEntity));
        }

        /// <summary>
        /// Save a ExpandoObject entity to Azure Table Storage asynchronously.
        /// </summary>
        /// <param name="entity">The ExpandoObject entity to save.</param>
        /// <param name="partitionKey">The Azure Table Storage partition key.</param>
        /// <param name="rowKey">The Azure Table Storage row key.</param>
        /// <returns>A Task to save the entity asynchronously.</returns>
        /// <remarks>
        /// ExpandoObject values must be of the following types: float, decimal, double, int, short, byte, string, bool, DateTime, Guid, or Uri. 
        /// All other types will be ignored.
        /// </remarks>
        public override async Task SaveEntityAsync(ExpandoObject entity, string partitionKey, string rowKey)
        {
           await Table.CreateIfNotExistsAsync();

            var dynamicTableEntity = entity.ToDynamicTableEntity(partitionKey, rowKey);
            await Table.ExecuteAsync(TableOperation.InsertOrReplace(dynamicTableEntity));
        }
    }
}
