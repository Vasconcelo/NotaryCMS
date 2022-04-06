namespace Common.Middleware.Authentication.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.Extensions.Options;    
    using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.SqlAzure;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    public class ClaimRepository : IdentityAdoRepositoryBase, IClaimRepository
    {
        public ClaimRepository(IOptions<AuthenticationOptions> options)
            : base(options)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "This code is internal.")]
        public IList<Claim> RetrieveBySubject(Guid subjectGuid)
        {
            var sql = new StringBuilder();
            sql.Append("WITH claimCTE AS ");
            sql.Append("(SELECT cv.ClaimId");
            sql.Append("	,cv.ParentClaimId");
            sql.Append("	,cv.ClaimTypeId");
            sql.Append("	,cv.ClaimValue ");
            sql.Append("FROM iDm.ClaimView cv");
            sql.Append("	INNER JOIN iDm.SubjectClaim sc ON cv.ClaimId = sc.ClaimId ");
            sql.Append("WHERE sc.SubjectGuid = @subjectGuid ");
            sql.Append("UNION ALL ");
            sql.Append("SELECT cv.ClaimId");
            sql.Append("	,cv.ParentClaimId");
            sql.Append("	,cv.ClaimTypeId");
            sql.Append("	,cv.ClaimValue ");
            sql.Append("FROM iDm.ClaimView cv");
            sql.Append("	INNER JOIN claimCTE ON cv.ParentClaimId = claimCTE.ClaimId) ");
            sql.Append("SELECT DISTINCT c.ClaimId");
            sql.Append("	,c.ClaimTypeId");
            sql.Append("	,ct.ClaimTypeName");
            sql.Append("	,ct.ClaimTypeUri");
            sql.Append("	,c.ClaimValue ");
            sql.Append("FROM claimCTE c");
            sql.Append("	INNER JOIN iDm.ClaimType ct ON c.ClaimTypeId = ct.ClaimTypeId");

            var retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(
                MaxRetries,
                TimeSpan.FromMilliseconds(MinBackoffDelay),
                TimeSpan.FromMilliseconds(MaxBackoffDelay),
                TimeSpan.FromMilliseconds(DeltaBackoffDelay));

            var connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(sql.ToString(), connection);

            var parm = command.CreateParameter();
            parm.ParameterName = "@subjectGuid";
            parm.Value = subjectGuid;
            parm.SqlDbType = SqlDbType.UniqueIdentifier;
            command.Parameters.Add(parm);

            var claims = new List<Claim>();

            try
            {
                connection.OpenWithRetry(retryPolicy);

                using (var dataReader = command.ExecuteReaderWithRetry(retryPolicy))
                {
                    while (dataReader.Read())
                    {
                        claims.Add(new Claim(dataReader["ClaimTypeUri"].ToString(), dataReader["ClaimValue"].ToString()));
                    }

                    return claims;
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
