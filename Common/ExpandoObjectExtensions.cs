namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Provides extensions to the ExpandoObject class.
    /// </summary>
    public static class ExpandoObjectExtensions
    {
        /// <summary>
        /// Create an Azure Table Storage DynamicTableEntity object from an ExpandoObject.
        /// </summary>
        /// <param name="obj">
        /// The ExpandoObject to use.
        /// </param>
        /// <param name="partitionKey">
        /// The Azure Table Storage partition key.
        /// </param>
        /// <param name="rowKey">
        /// The Azure Table Storage row key.
        /// </param>
        /// <remarks>
        /// ExpandoObject values must be of the following types: float, decimal, double, int, short, byte, string, bool, DateTime, Guid, or Uri. 
        /// All other types will be ignored.
        /// </remarks>
        /// <returns>
        /// The <see cref="DynamicTableEntity"/>.
        /// </returns>
        public static DynamicTableEntity ToDynamicTableEntity(this ExpandoObject obj, string partitionKey, string rowKey)
        {
            var objDictionary = obj as IDictionary<string, object>;

            var timestampObj = objDictionary
                .FirstOrDefault(p => p.Key.Equals("Timestamp", StringComparison.Ordinal))
                .Value;

            DateTime timestamp;
            if (timestampObj == null)
            {
                timestamp = DateTime.UtcNow;
            }
            else
            {
                timestamp = (DateTime)timestampObj;
            }

            var dynamicTableEntity = new DynamicTableEntity(partitionKey, rowKey) { Timestamp = timestamp };

            foreach (var key in objDictionary.Keys)
            {
                var attributeValue = objDictionary[key];

                if (attributeValue is float)
                {
                    dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(key, new EntityProperty((float)attributeValue)));
                    continue;
                }

                if (attributeValue is decimal)
                {
                    dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(key, new EntityProperty(Convert.ToDouble(attributeValue, CultureInfo.InvariantCulture))));
                    continue;
                }

                if (attributeValue is double)
                {
                    dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(key, new EntityProperty((double)attributeValue)));
                    continue;
                }

                if (attributeValue is int)
                {
                    dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(key, new EntityProperty((int)attributeValue)));
                    continue;
                }

                if (attributeValue is short)
                {
                    dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(key, new EntityProperty((short)attributeValue)));
                    continue;
                }

                if (attributeValue is byte)
                {
                    dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(key, new EntityProperty((byte)attributeValue)));
                    continue;
                }

                if (attributeValue is string)
                {
                    dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(key, new EntityProperty(attributeValue.ToString())));
                    continue;
                }

                if (attributeValue is bool)
                {
                    dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(key, new EntityProperty((bool)attributeValue)));
                    continue;
                }

                if (attributeValue is DateTime)
                {
                    dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(key, new EntityProperty((DateTime)attributeValue)));
                    continue;
                }

                if (attributeValue is Guid)
                {
                    dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(key, new EntityProperty((Guid)attributeValue)));
                    continue;
                }

                if (attributeValue is Uri)
                {
                    dynamicTableEntity.Properties.Add(new KeyValuePair<string, EntityProperty>(key, new EntityProperty(attributeValue.ToString())));
                }
            }

            return dynamicTableEntity;
        }
    }
}
