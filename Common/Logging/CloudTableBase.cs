namespace Common.Logging
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// A base class that provides functionality to store entities in Azure Table Storage.
    /// </summary>
    /// <typeparam name="T">The type of entity to store.</typeparam>
    public abstract class CloudTableBase<T> where T : IEnumerable
    {
        private readonly CloudTable _table;

        /// <summary>
        /// Initializes a new instance of the CloudTableBase class.
        /// </summary>
        /// <param name="connectionString">A connection string for the storage account.</param>
        /// <param name="tableName">The name of the table that will store the objects.</param>
        protected CloudTableBase(string connectionString, string tableName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            TableName = tableName;

            var options = new TableRequestOptions { PayloadFormat = TablePayloadFormat.JsonNoMetadata };
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            var servicePoint = ServicePointManager.FindServicePoint(storageAccount.TableEndpoint);
            servicePoint.UseNagleAlgorithm = false;
            servicePoint.Expect100Continue = false;
            servicePoint.ConnectionLimit = 10;

            var tableClient = storageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions = options;
            _table = tableClient.GetTableReference(tableName);
        }

        /// <summary>
        /// Gets the name of the table that stores the objects.
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Gets the Azure Table Storage table that stores the objects.
        /// </summary>
        protected CloudTable Table
        {
            get
            {
                return _table;
            }
        }

        /// <summary>
        /// Save an entity to Azure Table Storage.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        /// <param name="partitionKey">The Azure Table Storage partition key.</param>
        /// <param name="rowKey">The Azure Table Storage row key.</param>
        public abstract void SaveEntity(T entity, string partitionKey, string rowKey);

        /// <summary>
        /// Save an entity to Azure Table Storage asynchronously.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        /// <param name="partitionKey">The Azure Table Storage partition key.</param>
        /// <param name="rowKey">The Azure Table Storage row key.</param>
        /// <returns>A Task to save the entity asynchronously.</returns>
        public abstract Task SaveEntityAsync(T entity, string partitionKey, string rowKey);
    }
}
