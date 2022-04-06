namespace Common.Security
{
    using Microsoft.AspNetCore.Authorization;

    public class MultiTenantRequirement : IAuthorizationRequirement
    {
        public MultiTenantRequirement(bool isMultiTenant)
        {
            IsMultiTenant = isMultiTenant;
        }

        public bool IsMultiTenant { get; private set; }
    }
}
