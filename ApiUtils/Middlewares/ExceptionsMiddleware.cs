namespace ApiUtils.Middlewares;

using Google;
using Loggers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PlVisualizer.Api.Dto;
using PlVisualizer.Api.Dto.Exceptions;

/// <summary>
/// Represents middleware that handles exceptions occuring in application
/// </summary>
public class ExceptionsMiddleware
{
    private RequestDelegate next;
    private ILogger logger;
    
    public ExceptionsMiddleware(RequestDelegate next, ILogger logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Unhandled exception in api method {method} occured. {message}",
                context.Request.Method, exception.Message);
            switch (exception)
            {
                case PLVisualizerExceptionBase baseException:
                    await WriteExceptionAsync(context, baseException, baseException.StatusCode);
                    break;
                case GoogleApiException googleApiException:
                    await WriteExceptionAsync(context, googleApiException, 400);
                    break;
                default:
                    await WriteExceptionAsync(context, exception, 500);
                    break;
            }
        }
    }

    private static async Task WriteExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        var response = new Response(Content: exception.Message, Exception: exception);
        var result = JsonConvert.SerializeObject(response);
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
}