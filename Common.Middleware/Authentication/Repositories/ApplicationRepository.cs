namespace Common.Middleware.Authentication.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.Extensions.Options;    
    using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.SqlAzure;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    public class ApplicationRepository : IdentityAdoRepositoryBase, IApplicationRepository
    {
        public ApplicationRepository(IOptions<AuthenticationOptions> options)
            : base(options)
        {
        }

        public string RetrieveClientSecretById(string id)
        {
            const string Sql = "SELECT ClientSecret FROM iDm.Application WHERE ApplicationId = @id AND IsActive = 1";

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
            parm.SqlDbType = SqlDbType.NVarChar;
            command.Parameters.Add(parm);

            try
            {
                connection.OpenWithRetry(retryPolicy);

                using (var dataReader = command.ExecuteReaderWithRetry(CommandBehavior.SingleRow, retryPolicy))
                {
                    return dataReader.Read() ? dataReader.GetString(0) : null;
                }
            }
            finally
            {
                command.Dispose();
                connection.Dispose();
            }
        }

        public string RetrieveClientSecretByUri(string uri)
        {
            const string Sql = "SELECT ClientSecret FROM iDm.Application WHERE ApplicationUri = @uri AND IsActive = 1";

            var retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(
                MaxRetries,
                TimeSpan.FromMilliseconds(MinBackoffDelay),
                TimeSpan.FromMilliseconds(MaxBackoffDelay),
                TimeSpan.FromMilliseconds(DeltaBackoffDelay));

            var connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(Sql, connection);

            var parm = command.CreateParameter();
            parm.ParameterName = "@uri";
            parm.Value = uri;
            parm.SqlDbType = SqlDbType.VarChar;
            command.Parameters.Add(parm);

            try
            {
                connection.OpenWithRetry(retryPolicy);

                using (var dataReader = command.ExecuteReaderWithRetry(CommandBehavior.SingleRow, retryPolicy))
                {
                    return dataReader.Read() ? dataReader.GetString(0) : null;
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
