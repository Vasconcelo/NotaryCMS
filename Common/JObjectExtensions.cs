namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Provides extensions to the Newtonsoft JObject class.
    /// </summary>
    public static class JObjectExtensions
    {
        /// <summary>
        /// Create an Azure Table Storage DynamicTableEntity object from a JObject.
        /// </summary>
        /// <param name="obj">The JObject to use.</param>
        /// <param name="partitionKey">The Azure Table Storage partition key.</param>
        /// <param name="rowKey">The Azure Table Storage row key.</param>
        /// <remarks>
        /// JSON values must be of the following types: Date, Boolean, Float, Guid, Integer, String, or Uri. 
        /// All other types will be ignored.
        /// </remarks>
        /// <returns>
        /// The <see cref="DynamicTableEntity"/>.
        /// </returns>
        public static DynamicTableEntity ToDynamicTableEntity(this JObject obj, string partitionKey, string rowKey)
        {
            DateTime timestamp;
            var timestampObj = obj.Properties().FirstOrDefault(p => p.Name.Equals("Timestamp", StringComparison.Ordinal));
            if (timestampObj == null)
            {
                timestamp = DateTime.UtcNow;
            }
            else
            {
                timestamp = (DateTime)timestampObj.Value;
            }

            var dynamicTableEntity = new DynamicTableEntity(partitionKey, rowKey) { Timestamp = timestamp };

            foreach (var property in obj.Properties())
            {
                switch (property.Value.Type)
                {
                    case JTokenType.Date:
                        dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(property.Name, new EntityProperty((DateTime?)property.Value)));
                        break;

                    case JTokenType.Boolean:
                        dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(property.Name, new EntityProperty((bool)property.Value)));
                        break;

                    case JTokenType.Float:
                        dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(property.Name, new EntityProperty((double)property.Value)));
                        break;

                    case JTokenType.Guid:
                        dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(property.Name, new EntityProperty((Guid)property.Value)));
                        break;

                    case JTokenType.Integer:
                        dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(property.Name, new EntityProperty((int)property.Value)));
                        break;

                    case JTokenType.String:
                        dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(property.Name, new EntityProperty(property.Value.ToString())));
                        break;

                    case JTokenType.Uri:
                        dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(property.Name, new EntityProperty(property.Value.ToString())));
                        break;
                }
            }

            return dynamicTableEntity;
        }
    }
}
