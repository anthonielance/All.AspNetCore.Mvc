// Based on the work by Rick Strahl's
// https://weblog.west-wind.com/posts/2016/oct/16/error-handling-and-exceptionfilter-dependency-injection-for-aspnet-core-apis

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace All.AspNetCore.Mvc.Filters
{
    internal class ApiError
    {
        public ApiError(string message)
        {
            Message = message;
            IsError = true;
        }

        public ApiError(ModelStateDictionary modelState)
        {
            IsError = true;
            if (modelState != null && modelState.ErrorCount > 0)
            {
                Message = "Please correct the specified errors and try again.";
            }
        }


        public string Message { get; set; }
        public bool IsError { get; set; }
        public string Detail { get; set; }
        public ModelStateDictionary Errors { get; set; }
    }
}