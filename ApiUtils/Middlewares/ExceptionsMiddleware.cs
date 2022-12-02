using System.Reflection;
using Microsoft.AspNetCore.Http;
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
            context.Response.StatusCode = exceptionBase.StatusCode;
        }
        catch (Exception exception)
        {
            Console.WriteLine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            Console.WriteLine(exception);
            Console.WriteLine(exception.Message);
            context.Response.StatusCode = 500;
        }
    }
}