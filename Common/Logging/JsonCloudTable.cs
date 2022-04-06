namespace Common.Logging
{
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Provides functionality to store JSON entities in Azure Table Storage.
    /// </summary>
    public class JsonCloudTable : CloudTableBase<JObject>
    {
        /// <summary>
        /// Initializes a new instance of the NNA.Services.Common.Data.JsonCloudTable class.
        /// </summary>
        /// <param name="connectionString">A connection string for the storage account.</param>
        /// <param name="tableName">The name of the table that will store the JSON entities.</param>
        public JsonCloudTable(string connectionString, string tableName)
            : base(connectionString, tableName)
        {
        }

        /// <summary>
        /// Save a JSON entity to Azure Table Storage.
        /// </summary>
        /// <param name="entity">The JSON entity to save.</param>
        /// <param name="partitionKey">The Azure Table Storage partition key.</param>
        /// <param name="rowKey">The Azure Table Storage row key.</param>
        /// <remarks>
        /// JSON values must be of the following types: Date, Boolean, Float, Guid, Integer, String, or Uri. 
        /// All other types will be ignored.
        /// </remarks>
        public override void SaveEntity(JObject entity, string partitionKey, string rowKey)
        {
            Table.CreateIfNotExistsAsync();

            var dynamicTableEntity = entity.ToDynamicTableEntity(partitionKey, rowKey);
            Table.ExecuteAsync(TableOperation.InsertOrReplace(dynamicTableEntity));
        }

        /// <summary>
        /// Save a JSON entity to Azure Table Storage asynchronously.
        /// </summary>
        /// <param name="entity">The JSON entity to save.</param>
        /// <param name="partitionKey">The Azure Table Storage partition key.</param>
        /// <param name="rowKey">The Azure Table Storage row key.</param>
        /// <returns>A Task to save the entity asynchronously.</returns>
        /// <remarks>
        /// JSON values must be of the following types: Date, Boolean, Float, Guid, Integer, String, or Uri. 
        /// All other types will be ignored.
        /// </remarks>
        public override async Task SaveEntityAsync(JObject entity, string partitionKey, string rowKey)
        {
            Table.CreateIfNotExistsAsync();

            var dynamicTableEntity = entity.ToDynamicTableEntity(partitionKey, rowKey);
            await Table.ExecuteAsync(TableOperation.InsertOrReplace(dynamicTableEntity));
        }
    }
}
