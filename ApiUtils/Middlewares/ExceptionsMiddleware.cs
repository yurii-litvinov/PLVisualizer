using Microsoft.AspNetCore.Http;
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
        catch (PLVisualizerApiNotFoundException exception)
        {
            context.Response.StatusCode = 404;
        }
        catch (PLVisualizerApiBadRequestException exception)
        {
            context.Response.StatusCode = 400;
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = 500;
        }
    }
}