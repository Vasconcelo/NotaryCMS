using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Common.Middleware.Authentication
{
    public class CustomAuthenticationHandler : IAuthenticationHandler
    {
        private HttpContext _context;

        public async Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            _context = context;
        }

        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            return AuthenticateResult.NoResult();
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            _context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.FromResult(0);
        }

        public Task ForbidAsync(AuthenticationProperties properties) 
        {
            _context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.FromResult(0);
        }       
    }
}
