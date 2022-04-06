namespace Common.Middleware.Authentication.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using Microsoft.Extensions.Options;
    using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.SqlAzure;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using Security;

    public class UserAccountRepository : IdentityAdoRepositoryBase, IUserAccountRepository
    {
        public UserAccountRepository(IOptions<AuthenticationOptions> options)
            : base(options)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "The code is internal.")]
        public UserAccount RetrieveByUserAppId(string id)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT a2.UserId");
            sql.AppendLine("	,a2.UserTypeId");
            sql.AppendLine("	,a2.OAuthAccountGuid");
            sql.AppendLine("	,a2.IsActive");
            sql.AppendLine("	,a2.IsLocked");
            sql.AppendLine("	,a1.ApplicationId");
            sql.AppendLine("	,CAST(CASE WHEN os.Name = 'Active' THEN 1 ELSE 0 END AS BIT) AS  OrgIsActive");
            sql.AppendLine("	,o.OrganizationGuid ");
            sql.AppendLine("	,a2.SubjectGuid ");
            sql.AppendLine("FROM iDm.OAuthAccountApplication a1");
            sql.AppendLine("	INNER JOIN iDm.OAuthAccount a2 ON a1.OAuthAccountId = a2.OAuthAccountId");
            sql.AppendLine("	LEFT OUTER JOIN iDm.Organization o ON a2.OrganizationId = o.OrganizationId ");
            sql.AppendLine("	LEFT OUTER JOIN iDm.OrganizationStatus os ON o.OrganizationStatusId = os.OrganizationStatusId");
            sql.AppendLine("WHERE a1.OAuthAccountApplicationId = @id");

            var retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(
                MaxRetries,
                TimeSpan.FromMilliseconds(MinBackoffDelay),
                TimeSpan.FromMilliseconds(MaxBackoffDelay),
                TimeSpan.FromMilliseconds(DeltaBackoffDelay));

            var connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(sql.ToString(), connection);

            var parm = command.CreateParameter();
            parm.ParameterName = "@id";
            parm.Value = id;
            parm.SqlDbType = SqlDbType.VarChar;
            command.Parameters.Add(parm);

            try
            {
                connection.OpenWithRetry(retryPolicy);

                using (var dataReader = command.ExecuteReaderWithRetry(CommandBehavior.SingleRow, retryPolicy))
                {
                    if (dataReader.Read())
                    {
                        var orgIsActive = dataReader["OrgIsActive"] as bool?;
                        var userIsActive = (bool)dataReader["IsActive"];

                        var userAccount = new UserAccount
                        {
                            UserId = dataReader["UserId"].ToString(),
                            OAuthAccountGuid = (Guid)dataReader["OAuthAccountGuid"],
                            ApplicationId = dataReader["ApplicationId"].ToString(),
                            OrganizationGuid = dataReader["OrganizationGuid"] as Guid?,
                            IsLocked = (bool)dataReader["IsLocked"],
                            IsActive = userIsActive && (!orgIsActive.HasValue || orgIsActive.Value),
                            OAuthSubjectId = (Guid)dataReader["SubjectGuid"]
                        };

                        var userTypeId = dataReader["UserTypeId"] as int?;
                        userAccount.UserType = userTypeId.HasValue ? (UserType)Enum.ToObject(typeof(UserType), userTypeId.Value) : UserType.Unknown;

                        return userAccount;
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
    }
}
