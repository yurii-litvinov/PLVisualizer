using Loggers;
using Microsoft.AspNetCore.Http;

namespace ApiUtils.Middlewares;

public class RequestLoggingMiddleware
{
    private ILogger logger;
    private RequestDelegate next;
    
    public RequestLoggingMiddleware(ILogger logger, RequestDelegate next)
    {
        this.logger = logger;
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);
        logger.Info(
            "Request {method} {url} with response {statusCode}", 
            context.Request?.Method,
            context.Request?.Path,
            context.Response?.StatusCode);
    }
}