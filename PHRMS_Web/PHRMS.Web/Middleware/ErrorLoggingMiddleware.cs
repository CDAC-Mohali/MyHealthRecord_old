using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Logging;

namespace PHRMS.Web.Middleware
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ErrorLoggingMiddleware(RequestDelegate next,
        ILoggerFactory loggerFactory)
        { 
            _next = next;
            _logger = loggerFactory.CreateLogger<ErrorLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An unhandled exception has occurred: " + ex.Message, ex);
                throw; // Don't stop the error
            }
        }
    }
}
