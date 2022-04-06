namespace Common.Security
{    
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Microsoft.Extensions.Options;

    public class MultiTenantHandler : AuthorizationHandler<MultiTenantRequirement>
    {
        public const string NnaOrganizationId = "37FA1B99-6B14-4C52-823A-020C01FF2E53";
        private readonly RbacConfigurationOptions _rbacConfigurationOptions;

        public MultiTenantHandler(IOptions<RbacConfigurationOptions> rbacConfigurationOptions)
        {
            _rbacConfigurationOptions = rbacConfigurationOptions.Value;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MultiTenantRequirement requirement)
        {            
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.OrganizationId))
            {              
                context.Fail();
                return Task.FromResult(0);
            }

            var organizationId = context.User
                .FindFirst(c => c.Type == ClaimTypes.OrganizationId)
                .Value;

            var isNna = organizationId.Equals(NnaOrganizationId, StringComparison.OrdinalIgnoreCase);

            //If ClaimsAuthorizationEnabled is false then setting the claims authorization requirement to success.           
            if (!_rbacConfigurationOptions.ClaimsAuthorizationEnabled)
            {
                foreach (var item in context.Requirements)
                {
                    if (item is ClaimsAuthorizationRequirement)
                    {
                        context.Succeed(item);
                        break;
                    }
                }                
            }

            if (requirement.IsMultiTenant || isNna)
            {
                context.Succeed(requirement);                
            }            

            return Task.FromResult(0);
        }
    }
}
