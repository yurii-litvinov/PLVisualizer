namespace ApiUtils.Middlewares;

using Loggers;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

/// <summary>
/// Represents middleware that writes logs of input network requests
/// </summary>
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
            context.Request?.Method ?? throw new UnreachableException(),
            context.Request?.Path ?? throw new UnreachableException(),
            context.Response?.StatusCode ?? throw new UnreachableException());
    }
}