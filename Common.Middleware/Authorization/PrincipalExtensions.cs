using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NNA.Foundation.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Middleware.Authorization
{
    public static class PrincipalExtensions
    {
        private const string BypassFlag = "bypass-authentication";
        private const string AuthMethod = "authentication-method";
        private const string Token = "authentication-token";

        public static readonly TraceSwitch Switch = new TraceSwitch("AuthSwitch", "Auth switch.");


        public static string ParseKeySignature(this HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues header))
            {
                return ParseAuthorizationValue(header.FirstOrDefault(), "NNAKeySig");
            }
            return "";
        }

        public static string ParseOAuthBearerToken(this HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues header))
            {
                return ParseAuthorizationValue(header.FirstOrDefault(), "Bearer");
            }
            return "";
        }

        public static string ParseNNADate(this HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(Configuration.DateLabel, out StringValues header))
            {
                return header.FirstOrDefault().Trim();
            }
            return null;
        }

        public static string ParseAccessToken(this HttpContext context)
        {
            var token = context.Request.Query["code"];

            if (token.Any()) return token.FirstOrDefault();

            token = context.Request.Cookies["accessToken"];

            return token.Any() ? token.FirstOrDefault() : "";
        }

        public static string ParseApiKey(this HttpContext context)
        {
            var token = "";
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues header))
            {
                token = header.Any() ? "" : ParseAuthorizationValue(header.FirstOrDefault(), "ApiKey");

                // LCheck for an alternate version in the query string
                token = string.IsNullOrWhiteSpace(token) ? context.Request.Query["api_key"].FirstOrDefault() : token;

                // Check for an alternate version in the query string
                token = string.IsNullOrWhiteSpace(token) ? context.Request.Query["key"].FirstOrDefault() : token;

                // Check for the key in the headers
                token = string.IsNullOrWhiteSpace(token) ? context.Request.Headers["api_key"].FirstOrDefault() : token;

            }
            return token;
        }

        public static void SetBypassFlag(this HttpContext context)
        {
            context.Items?.Add(BypassFlag, true);
        }

        public static bool BypassAuthentication(this HttpContext context)
        {
            var bypassFlagValue = context.Items?.FirstOrDefault(x => BypassFlag.Equals(x.Key));

            return bypassFlagValue.HasValue && bool.Parse(bypassFlagValue.GetValueOrDefault().Value.ToString());
        }

        public static void SetAuthMethod(this HttpContext context, string method)
        {
            context.Items?.Add(AuthMethod, method);
        }

        public static string GetAuthMethod(this HttpContext context)
        {
            var authMethodValue = context.Items?.FirstOrDefault(x => AuthMethod.Equals(x.Key));

            return authMethodValue.HasValue ? authMethodValue.GetValueOrDefault().Value.ToString() : "";
        }

        public static void SetToken(this HttpContext context, string key)
        {
            context.Items?.Add(Token, key);
        }

        public static string GetToken(this HttpContext context)
        {
            var tokenValue = context.Items?.FirstOrDefault(x => Token.Equals(x.Key));

            return tokenValue.HasValue ? tokenValue.GetValueOrDefault().Value.ToString() : "";
        }

        private static string ParseAuthorizationValue(string header, string type)
        {
            type = type.Trim() + " ";

            return string.CompareOrdinal(header, 0, type, 0, type.Length) == 0 ? header.Remove(0, type.Length) : "";
        }
    }
}
