namespace Common.Middleware.Authentication.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.Extensions.Options;
    using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.SqlAzure;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    public class IdentityProviderRepository : IdentityAdoRepositoryBase, IIdentityProviderRepository
    {
        public IdentityProviderRepository(IOptions<AuthenticationOptions> options)
            : base(options)
        {
        }

        public IdentityProvider RetrieveByIssuerUri(string issuerUri)
        {
            const string Sql = "SELECT IdentityProviderName, AppIdClaimTypeUri, AppUriClaimTypeUri, UserAppIdClaimTypeUri, InvalidateTokenAtLogoff FROM iDm.IdentityProvider WHERE IssuerUri = @issuerUri AND IsActive = 1";

            var retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(
                MaxRetries,
                TimeSpan.FromMilliseconds(MinBackoffDelay),
                TimeSpan.FromMilliseconds(MaxBackoffDelay),
                TimeSpan.FromMilliseconds(DeltaBackoffDelay));

            var connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(Sql, connection);

            var parm = command.CreateParameter();
            parm.ParameterName = "@issuerUri";
            parm.Value = issuerUri;
            parm.SqlDbType = SqlDbType.NVarChar;
            command.Parameters.Add(parm);

            try
            {
                connection.OpenWithRetry(retryPolicy);

                using (var dataReader = command.ExecuteReaderWithRetry(CommandBehavior.SingleRow, retryPolicy))
                {
                    if (!dataReader.HasRows)
                    {
                        return null;
                    }

                    dataReader.Read();

                    return new IdentityProvider
                    {
                        Id = issuerUri.Replace(":", string.Empty),
                        IdentityProviderName = dataReader["IdentityProviderName"].ToString(),
                        IssuerUri = issuerUri,
                        AppIdClaimTypeUri = dataReader["AppIdClaimTypeUri"].ToString(),
                        AppUriClaimTypeUri = dataReader["AppUriClaimTypeUri"].ToString(),
                        UserAppIdClaimTypeUri = dataReader["UserAppIdClaimTypeUri"].ToString(),
                        InvalidateTokenAtLogoff = (bool)dataReader["InvalidateTokenAtLogoff"]
                    };
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
