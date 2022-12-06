using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PlVisualizer.Api.Dto.Exceptions;
using PlVisualizer.Api.Dto.Exceptions.ApiExceptions;

namespace ApiUtils.Middlewares;

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
        catch (PLVisualizerExceptionBase exceptionBase)
        {
            // add switch case some day ?
            await WriteExceptionAsync(context, exceptionBase, exceptionBase.StatusCode);
        }
        catch (Exception exception)
        {
            await WriteExceptionAsync(context, exception, 500);
        }
    }

    private static async Task WriteExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        var result = JsonConvert.SerializeObject(exception);
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
}