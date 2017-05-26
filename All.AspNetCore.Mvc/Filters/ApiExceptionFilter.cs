// Based on the work by Rick Strahl's
// https://weblog.west-wind.com/posts/2016/oct/16/error-handling-and-exceptionfilter-dependency-injection-for-aspnet-core-apis

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace All.AspNetCore.Mvc.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            ApiError apiError = null;
            if (context.Exception is ApiException)
            {
                var ex = context.Exception as ApiException;
                context.Exception = null;
                apiError = new ApiError(ex.Message)
                {
                    Errors = ex.Errors
                };
                context.HttpContext.Response.StatusCode = ex.StatusCode;

                _logger.LogWarning($"Application thrown error: {ex.Message}", ex);
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("Unauthorized Access");

                context.HttpContext.Response.StatusCode = 401;

                _logger.LogWarning("Unauthorized Access in Controller Filter.");
            }
            else
            {

#if !DEBUG
                var message = "An unhandled error occurred.";
                string stack = null;
#else
                var message = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;
#endif

                apiError = new ApiError(message)
                {
                    Detail = stack
                };
                context.HttpContext.Response.StatusCode = 500;

                _logger.LogError(new EventId(0), context.Exception, message);
            }

            context.Result = new JsonResult(apiError);

            base.OnException(context);
        }
    }
}
