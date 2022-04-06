//using Common.Middleware.Authentication;
//using Common.Utilities;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Controllers;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.Options;
//using NNA.Foundation.Caching;
//using NNA.Foundation.Caching.Utilities;
//using NNA.Foundation.Common.Models;
////using NNA.Foundation.Security.Models;
//using NNA.Foundation.Security.RBAC;
//using NNA.Foundation.Security.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using NNA.Foundation.Security.RBAC.Services;
//using NNA.Foundation.Security.RBAC.DataObjects;
//using NNA.Foundation.Security.RBAC.Repositories;

//namespace Common.Middleware.Authorization
//{
//    public class NnaAuthorizationFilter : IAuthorizationFilter
//    {
//        //private readonly ICachingService _cachingService;
//        private static readonly bool _cacheEnabled;
//        private static readonly ICacheService<Guid, AuthorizationSummary> AuthorizationCachingService;
//        private static readonly ICacheService<string, OperationSummaries> OperationsCachingService;

//        //private readonly AuthorizationService _authService;
//        //private readonly RbacConfigurationOptions _rbacConfigurationOptions;
//        private readonly AuthenticationOptions _authenticationOptions;

//        private readonly RbacAuthorizationService _rbacAuthorizationService;
//        private readonly RbacRepository _rbacRepository;

//        public bool Enabled { get; set; }
//        public AuthorizeMode AuthorizeMode { get; }

//        static NnaAuthorizationFilter()
//        {
//            //var redisConnectionString = ConfigurationUtils.GetStringValueSafely("Redis.Connection",
//            //    RedisUtils.GetRedisConnectionString(ConfigurationUtils.GetStringValueSafely("Redis.Host"),
//            //        ConfigurationUtils.GetStringValueSafely("Redis.Password")));

//            if (!ConfigurationUtils.TryGetStringValue("Redis.Connection", out var redisConnectionString, true))
//            {
//                if (ConfigurationUtils.TryGetStringValue("Redis.Host", out var redisHost, true) &&
//                    ConfigurationUtils.TryGetStringValue("Redis.Password", out var redisPassword, true))
//                    redisConnectionString = RedisUtils.GetRedisConnectionString(redisHost, redisPassword);
//            }

//            _cacheEnabled = !string.IsNullOrEmpty(redisConnectionString);

//            if (_cacheEnabled)
//            {
//                //_cachingService = new RedisCachingService(_rbacConfigurationOptions.RedisHost, _rbacConfigurationOptions.RedisPassword);
//                AuthorizationCachingService = new StackExchangeRedisCacheService<Guid, AuthorizationSummary>(redisConnectionString);
//                OperationsCachingService = new StackExchangeRedisCacheService<string, OperationSummaries>(redisConnectionString);
//            }
//        }

//        public NnaAuthorizationFilter(bool enabled = true,
//                                     AuthorizeMode authorizeMode = AuthorizeMode.Attribute,
//                                     IOptions<RbacConfigurationOptions> rbacConfigurationOptions = null,
//                                     IOptions<AuthenticationOptions> authenticationOptions = null)
//        {
//            AuthorizeMode = authorizeMode;

//            //_rbacConfigurationOptions = rbacConfigurationOptions.Value;
//            _authenticationOptions = authenticationOptions.Value;
//            Enabled = enabled && RbacConfigSettings.Instance.RbacAuthorizationEnabled; //_rbacConfigurationOptions.RbacAuthorizationEnabled;

//            //var httpClient = HttpClientFactory.Create();
//            //_authService = new AuthorizationService(httpClient, NNA.Foundation.Common.Configuration.IdentityServiceUrl, _authenticationOptions.IdentityAdo);

//            if (Enabled)
//            {
//                _rbacAuthorizationService = new RbacAuthorizationService();
//                _rbacRepository = new RbacRepository();
//            }
//        }

//        public void OnAuthorization(AuthorizationFilterContext context)
//        {
//            if (!Enabled)
//                return;

//            try
//            {
//                var principal = context.HttpContext.User;

//                if (AllowsAnonymous(context) && AuthorizeMode == AuthorizeMode.Filter)
//                    return;

//                if (principal?.Identity == null || !principal.Identity.IsAuthenticated)
//                    return;

//                var httpContext = context.HttpContext;
//                var authMethod = httpContext.GetAuthMethod();

//                if (authMethod == "accesstoken")
//                    return;

//                var session = new SessionInfo(principal);
//                var token = httpContext.GetToken();

//                if (ValidateOperationAuthorizeDisabled(context).GetAwaiter().GetResult() ||
//                    IsEndpointNewOperation(context).GetAwaiter().GetResult())
//                    return;

//                if (CheckAccess(session, context, token, authMethod).GetAwaiter().GetResult())
//                    return;

//                context.Result = new UnauthorizedResult();                
//            }
//            catch (Exception ex)
//            {
//                context.Result = new ContentResult
//                {
//                    StatusCode = 500,
//                    Content = ex.Message
//                };
//            }
//        }

//        private static bool AllowsAnonymous(AuthorizationFilterContext actionContext)
//        {
//            var controllerActionDescriptor = actionContext.ActionDescriptor as ControllerActionDescriptor;
//            if (controllerActionDescriptor != null)
//            {
//                var allowAnonymousAttribute =
//                    controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute),
//                        inherit: true);

//                if (allowAnonymousAttribute.Any())
//                    return true;

//                var authorizeAttributeAttribute =
//                    controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(AuthorizeAttribute),
//                        inherit: true);

//                if (authorizeAttributeAttribute.Any())
//                    return false;

//                var nnaAuthorizeAttribute =
//                    controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(NnaAuthorizeAttribute),
//                        inherit: true);

//                if (nnaAuthorizeAttribute.Any() && nnaAuthorizeAttribute[0] is NnaAuthorizeAttribute)
//                {
//                    var v = (NnaAuthorizeAttribute) nnaAuthorizeAttribute[0];
//                    if (v.Enabled && v.AuthorizeMode == AuthorizeMode.Attribute)
//                        return false;

//                    //if (!v.Enabled || v.AuthorizeMode != AuthorizeMode.Attribute)
//                    //  return true;
//                }

//                return true;
//            }

//            return false;
//        }

//        private async Task<bool> CheckAccess(ISessionInfo session, AuthorizationFilterContext actionContext, string token, string authMethod)
//        {
//            AuthorizationSummary summary;

//            //if (session.UserId != Guid.Empty && NNA.Foundation.Common.Configuration.RedisEnabled && _cacheEnabled && RbacConfigSettings.Instance.RbacCacheExpirationSeconds > 0)
//            //{
//            //    //summary = await GetCachedSummary(session.UserId, token, authMethod);
//            //    summary = await AuthorizationCachingService.GetOrSetAsync(session.UserId, async id => await _rbacAuthorizationService.GetUserAuthorizationSummaryAsync(new NNA.Foundation.Security.Models.AuthenticationInformation(authMethod, token)));

//            //    // if we're impersonating, make sure we aren't loading user's summary from cache
//            //    if (summary != null
//            //        && IsActivelyImpersonating(summary.Impersonation)
//            //        && (summary.Id == summary.Impersonation.UserId || summary.Id != summary.Impersonation.TargetUserId))
//            //    {
//            //        //summary = await _authService.GetUserAuthorizationSummary(token, authMethod);
//            //        summary = await _rbacAuthorizationService.GetUserAuthorizationSummaryAsync(new NNA.Foundation.Security.Models.AuthenticationInformation(authMethod, token));
//            //    }
//            //}
//            //else
//            //{
//            //    //summary = await _authService.GetUserAuthorizationSummary(token, authMethod);
//            //    summary = await _rbacAuthorizationService.GetUserAuthorizationSummaryAsync(new NNA.Foundation.Security.Models.AuthenticationInformation(authMethod, token));
//            //}

//            // let caching get handled in identity svc only
//            summary = await _rbacAuthorizationService.GetUserAuthorizationSummaryAsync(new NNA.Foundation.Security.Models.AuthenticationInformation(authMethod, token));

//            if (summary == null)
//                throw new Exception($"Summary not found for user with OAuthAccountId '{session.UserId}' using token '{token}'.");

//            if (summary.Operations == null)
//                return false;

//            var controllerActionDescriptor = actionContext.ActionDescriptor as ControllerActionDescriptor;
//            var resource = controllerActionDescriptor.ControllerTypeInfo.FullName;
//            var action = controllerActionDescriptor.ActionName;
//            var method = actionContext.HttpContext.Request.Method;

//            return summary.Operations
//                .Any(x => x.Resource.Equals(resource, StringComparison.InvariantCultureIgnoreCase)
//                          && x.Action.Equals(action, StringComparison.InvariantCultureIgnoreCase)
//                          && x.Method.Equals(method, StringComparison.InvariantCultureIgnoreCase));
//        }

//        private static bool IsActivelyImpersonating(Impersonation impersonation)
//        {
//            return impersonation != null
//                   && impersonation.Expires > DateTime.UtcNow
//                   && impersonation.Expires > impersonation.Initiated
//                   && impersonation.UserId != Guid.Empty
//                   && impersonation.TargetUserId != Guid.Empty
//                   && impersonation.UserId != impersonation.TargetUserId;
//        }

//        private async Task<bool> ValidateOperationAuthorizeDisabled(AuthorizationFilterContext actionContext)
//        {
//            string authorizationDisabledKey = "AuthorizationDisabledOperations";

//            //List<OperationSummary> operationSummaries = null;
//            OperationSummaries operationSummaries = null;

//            var getAuthorizeDisabledOperations = new Func<string, Task<OperationSummaries>>(async id =>
//            {
//                var summaries = await _rbacRepository.RetrieveOperationsByAuthorizationDisabledAsync(true);
//                if (summaries == null)
//                    return null;

//                return new OperationSummaries(summaries, id);
//            });

//            if (NNA.Foundation.Common.Configuration.RedisEnabled && _cacheEnabled && RbacConfigSettings.Instance.RbacCacheExpirationSeconds > 0)
//            {
//                operationSummaries = await OperationsCachingService.GetOrSetAsync(authorizationDisabledKey, getAuthorizeDisabledOperations);
//            }
//            else
//            {
//                //operationSummaries = await _authService.GetOperationsByAuthorizationDisabled(true);
//                operationSummaries = await getAuthorizeDisabledOperations(authorizationDisabledKey);
//            }

//            if (operationSummaries == null || !operationSummaries.Any())
//                return false;

//            var controllerActionDescriptor = actionContext.ActionDescriptor as ControllerActionDescriptor;
//            var resource = controllerActionDescriptor?.ControllerTypeInfo.FullName;
//            var action = controllerActionDescriptor?.ActionName;
//            var method = actionContext.HttpContext.Request.Method;

//            return operationSummaries
//                .Any(x => x.Resource.Equals(resource, StringComparison.InvariantCultureIgnoreCase)
//                          && x.Action.Equals(action, StringComparison.InvariantCultureIgnoreCase)
//                          && x.Method.Equals(method, StringComparison.InvariantCultureIgnoreCase));
//        }


//        private async Task<bool> IsEndpointNewOperation(AuthorizationFilterContext actionContext)
//        {
//            //match against new operations (last 90 days) without assigned permissions

//            string newRbacOperationsKey = "RbacNewOperations";

//            OperationSummaries operationSummaries = null;

//            var getNewOperations = new Func<string, Task<OperationSummaries>>(async cacheKey =>
//            {
//                var output = new OperationSummaries(await (_rbacRepository.RetrieveNewOperationsAsync(90)), cacheKey);
//                return output;
//            });

//            //if (RedisEnabled && RbacConfigSettings.Instance.RbacCacheEnabled)
//            if (NNA.Foundation.Common.Configuration.RedisEnabled && _cacheEnabled && RbacConfigSettings.Instance.RbacCacheExpirationSeconds > 0)
//            {
//                operationSummaries = await OperationsCachingService.GetOrSetAsync(newRbacOperationsKey, getNewOperations,
//                    TimeSpan.FromSeconds(RbacConfigSettings.Instance.RbacCacheExpirationSeconds));
//            }
//            else
//            {
//                // if cache is disabled or unavailable, just get operations directly
//                operationSummaries = await getNewOperations(newRbacOperationsKey);
//            }

//            if (operationSummaries == null || !operationSummaries.Any())
//                return false;

//            var controllerActionDescriptor = actionContext.ActionDescriptor as ControllerActionDescriptor;
//            var resource = controllerActionDescriptor?.ControllerTypeInfo.FullName;
//            var action = controllerActionDescriptor?.ActionName;
//            var method = actionContext.HttpContext.Request.Method;

//            //var authorized = operationSummaries?.Any(e =>
//            //      IsMatch(path, TokensToWildcards(e.Uri)) &&
//            //      e.Method.Equals(method.Method, StringComparison.InvariantCultureIgnoreCase)) ?? false;

//            var authorized = operationSummaries.Any(x =>
//                x.Resource.Equals(resource, StringComparison.InvariantCultureIgnoreCase)
//                && x.Action.Equals(action, StringComparison.InvariantCultureIgnoreCase)
//                && x.Method.Equals(method, StringComparison.InvariantCultureIgnoreCase));

//            return authorized;
//        }

//    }
//}
