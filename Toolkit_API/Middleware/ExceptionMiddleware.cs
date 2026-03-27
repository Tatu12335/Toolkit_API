using Toolkit_API.Middleware.Exceptions;

namespace Toolkit_API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger <ExceptionMiddleware> _logger;
        private readonly RequestDelegate _requestDelegate;
        public ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (AppException apex)
            {
                _logger.LogWarning(apex.Message);
                context.Response.StatusCode = apex.StatusCode;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = apex.Message
                });
                
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Internal server error"
                });
                
            }
        }
    }
}
