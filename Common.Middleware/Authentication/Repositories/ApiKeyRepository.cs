namespace Common.Middleware.Authentication.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.Extensions.Options;    
    using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.SqlAzure;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    public class ApiKeyRepository : IdentityAdoRepositoryBase, IApiKeyRepository
    {
        public ApiKeyRepository(IOptions<AuthenticationOptions> options)
            : base(options)
        {
        }

        public ApiKeyApp RetrieveById(Guid id)
        {
            const string Sql = "SELECT ak.IsActive, ak.ExpiresUtc, ak.ReferenceGuid, a.PublisherGuid, a.ApplicationId FROM iDm.ApiKey ak INNER JOIN iDm.Application a ON ak.ApplicationId = a.ApplicationId WHERE ApiKeyId = @id AND a.IsActive = 1";

            var retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(
                MaxRetries,
                TimeSpan.FromMilliseconds(MinBackoffDelay),
                TimeSpan.FromMilliseconds(MaxBackoffDelay),
                TimeSpan.FromMilliseconds(DeltaBackoffDelay));

            var connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(Sql, connection);

            var parm = command.CreateParameter();
            parm.ParameterName = "@id";
            parm.Value = id;
            parm.SqlDbType = SqlDbType.UniqueIdentifier;
            command.Parameters.Add(parm);

            try
            {
                connection.OpenWithRetry(retryPolicy);

                using (var dataReader = command.ExecuteReaderWithRetry(CommandBehavior.SingleRow, retryPolicy))
                {
                    if (dataReader.Read())
                    {
                        return new ApiKeyApp
                        {
                            Id = id,
                            IsActive = (bool)dataReader["IsActive"],
                            ExpiresUtc = dataReader["ExpiresUtc"] as DateTime?,
                            ReferenceGuid = dataReader["ReferenceGuid"] as Guid?,
                            PublisherGuid = (Guid)dataReader["PublisherGuid"],
                            ApplicationId = dataReader["ApplicationId"].ToString()
                        };
                    }

                    return null;
                }
            }
            finally
            {
                command.Dispose();
                connection.Dispose();
            }
        }

        public string RetrieveKeyById(Guid id)
        {
            const string Sql = "SELECT ApiKey FROM iDm.ApiKey WHERE ApiKeyId = @id AND IsActive = 1";

            var retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(
                MaxRetries,
                TimeSpan.FromMilliseconds(MinBackoffDelay),
                TimeSpan.FromMilliseconds(MaxBackoffDelay),
                TimeSpan.FromMilliseconds(DeltaBackoffDelay));

            var connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(Sql, connection);

            var parm = command.CreateParameter();
            parm.ParameterName = "@id";
            parm.Value = id;
            parm.SqlDbType = SqlDbType.UniqueIdentifier;
            command.Parameters.Add(parm);

            try
            {
                connection.OpenWithRetry(retryPolicy);

                using (var dataReader = command.ExecuteReaderWithRetry(CommandBehavior.SingleRow, retryPolicy))
                {
                    return dataReader.Read() ? dataReader[0].ToString() : null;
                }
            }
            finally
            {
                command.Dispose();
                connection.Dispose();
            }
        }
    }
}
