//namespace Common.Middleware.Correlation
//{
//    using System;
//    using System.Security.Claims;
//    using System.Threading.Tasks;
//    using Data;
//    using Logging;
//    using Microsoft.AspNetCore.Builder;
//    using Microsoft.AspNetCore.Http;
//    using Microsoft.Extensions.Options;

//    public class CorrelationMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly TableStorageConnectionStrings _connectionStrings;

//        public CorrelationMiddleware(
//             RequestDelegate next,
//             IOptions<TableStorageConnectionStrings> connectionStrings)
//        {
//            _next = next;
//            _connectionStrings = connectionStrings.Value;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            var messageId = GetMessageId(context);

//            try
//            {
//                var principal = context.User;
//                if (principal != null)
//                {
//                    var identity = (ClaimsIdentity)principal.Identity;
//                    identity.AddClaim(new Claim(Security.ClaimTypes.MessageId, messageId, "System.String"));
//                }
//            }
//            catch (Exception e)
//            {
//                await ServiceErrorHelper.LogErrorAsync(_connectionStrings.LogStorage, "Notary.CMS.Api", "CorrelationMiddleware", "Invoke", Guid.NewGuid(), e);
//                throw;
//            }

//            await _next.Invoke(context);
//        }

//        private static string GetMessageId(HttpContext context)
//        {
//            string messageId;
//            try
//            {
//                messageId = context.Request.Headers["nna-message-id"];

//                Guid guidCheck;
//                if (!Guid.TryParse(messageId, out guidCheck))
//                {
//                    messageId = Guid.NewGuid().ToString();
//                }
//            }
//            catch
//            {
//                messageId = Guid.NewGuid().ToString();
//            }

//            if (string.IsNullOrWhiteSpace(messageId))
//            {
//                messageId = Guid.NewGuid().ToString();
//            }

//            return messageId;
//        }
//    }
//}
