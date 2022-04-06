namespace Common.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security;
    using System.Security.Claims;
    using System.Threading;
    using Logging;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Security;
    using Validation;
    using Common;

    public class LoggingExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private const string ErrorCodeName = Globals.ErrorCodeLabel;
        private const string MessageIdName = Globals.MessageIdLabel;
        private const string TableName = "ServiceError";

        private readonly IExceptionLogger _exceptionLogger;

        public LoggingExceptionFilterAttribute(
            IExceptionLogger exceptionLogger)
        {
            _exceptionLogger = exceptionLogger;
        }

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var errorCode = ServiceErrorHelper.GetErrorCode(exception);
            var status = HttpStatusCode.InternalServerError;

            if (exception is ValidationException)
            {
                status = HttpStatusCode.BadRequest;
            }

            if (exception is ExceptionBase)
            {
                var errorCodeAsString = errorCode.ToString(CultureInfo.InvariantCulture);
                var errorCodeSfx = int.Parse(errorCodeAsString.PadRight(4), CultureInfo.InvariantCulture);

                if (errorCodeSfx < 5000)
                {
                    status = HttpStatusCode.BadRequest;
                }
            }

            if (exception.GetType().ToString().EndsWith("NotFoundException", StringComparison.OrdinalIgnoreCase))
            {
                status = HttpStatusCode.NotFound;
            }

            if (exception is InvalidUsernamePasswordException)
            {
                status = HttpStatusCode.Unauthorized;
            }

            if (exception is InvalidUsernamePasscodeException)
            {
                status = HttpStatusCode.Unauthorized;
            }

            if (exception is PasswordExpiredException)
            {
                status = HttpStatusCode.Unauthorized;
            }

            if (exception is PasscodeExpiredException)
            {
                status = HttpStatusCode.Unauthorized;
            }

            if (exception is AccountLockedException)
            {
                status = HttpStatusCode.Unauthorized;
            }

            if (exception is AccountLockedByAdminException)
            {
                status = HttpStatusCode.Unauthorized;
            }

            if (exception is NotImplementedException)
            {
                status = HttpStatusCode.NotImplemented;
            }

            if (exception is ArgumentException)
            {
                status = HttpStatusCode.BadRequest;
            }

            if (exception is SecurityException)
            {
                status = HttpStatusCode.Unauthorized;
            }

            var principal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (principal == null)
            {
                status = HttpStatusCode.Unauthorized;
            }

            var messageId = Guid.NewGuid();
            var identity = (ClaimsIdentity)principal.Identity;

            var messageIdClaim = identity.Claims
                .FirstOrDefault(c => c.Type.Equals(Security.ClaimTypes.MessageId));

            if (messageIdClaim != null)
            {
                Guid.TryParse(messageIdClaim.Value, out messageId);
            }

            var exceptionMessage = new HttpResponseMessage(status);
            exceptionMessage.Content = new StringContent(exception.Message.PadLeft(255));

            exceptionMessage.Headers.Add(
                ErrorCodeName,
                new List<string> { errorCode.ToString(CultureInfo.InvariantCulture) });

            if (principal != null)
            {
                exceptionMessage.Headers.Add(
                    MessageIdName,
                    new List<string> { messageId.ToString() });
            }

            var actionName = context.RouteData.Values["action"].ToString();
            var controllerName = context.RouteData.Values["controller"].ToString();

            _exceptionLogger.Log(
                $"{controllerName}Controller",
                actionName,
                messageId,
                exception);

            context.HttpContext.Response.StatusCode = (int)status;
            context.HttpContext.Response.Headers.Add(ErrorCodeName, errorCode.ToString(CultureInfo.InvariantCulture));

            if (principal != null)
            {
                exceptionMessage.Headers.Add(MessageIdName, messageId.ToString());
            }

            context.Result = new JsonResult(new { message = exception.Message.PadLeft(255) });
            context.Exception = null;
            base.OnException(context);
        }
    }
}
