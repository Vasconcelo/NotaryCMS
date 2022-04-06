namespace Common.Middleware.Authentication.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;

    public interface IClaimRepository
    {
        IList<Claim> RetrieveBySubject(Guid subjectGuid);
    }
}
