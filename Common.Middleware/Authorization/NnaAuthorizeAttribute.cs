using Microsoft.AspNetCore.Mvc;
using System;

namespace Common.Middleware.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class NnaAuthorizeAttribute : TypeFilterAttribute
    {
        public bool Enabled { get; set; }
        public AuthorizeMode AuthorizeMode { get; }

        public NnaAuthorizeAttribute(bool enabled = true, AuthorizeMode authorizeMode = AuthorizeMode.Attribute)
            : base(typeof(NnaAuthorizationFilter))
        {
            Enabled = enabled;
            AuthorizeMode = authorizeMode;
            Arguments = new object[] { enabled, authorizeMode };
        }
    }
}
