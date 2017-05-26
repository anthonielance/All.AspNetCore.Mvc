// Based on the work by Rick Strahl's
// https://weblog.west-wind.com/posts/2016/oct/16/error-handling-and-exceptionfilter-dependency-injection-for-aspnet-core-apis

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace All.AspNetCore.Mvc.Filters
{
    internal class ApiException : Exception
    {

        public ApiException(string message, int statusCode = 500, ModelStateDictionary errors = null) : base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }

        public ApiException(Exception ex, int statusCode = 500) : base(ex.Message)
        {
            StatusCode = statusCode;
        }

        public ModelStateDictionary Errors { get; set; }
        public int StatusCode { get; set; }
    }
}