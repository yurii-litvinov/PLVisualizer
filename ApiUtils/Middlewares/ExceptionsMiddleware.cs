using System.Reflection;
using Google;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PlVisualizer.Api.Dto;
using PlVisualizer.Api.Dto.Exceptions;
using PlVisualizer.Api.Dto.Exceptions.ApiExceptions;

namespace ApiUtils.Middlewares;

/// <summary>
/// Represents middleware that handles exceptions occuring in application
/// </summary>
public class ExceptionsMiddleware
{
    private RequestDelegate next;
    
    public ExceptionsMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (exception is PLVisualizerExceptionBase baseException)
            {
                await WriteExceptionAsync(context, baseException, baseException.StatusCode);
            }
            else if (exception is GoogleApiException googleApiException)
            {
                
            }
            else
            {
                await WriteExceptionAsync(context, exception, 500);
            }
        }
    }

    private static async Task WriteExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        var response = new Response { Content = exception.Message, Exception = exception};
        var result = JsonConvert.SerializeObject(response);
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
}