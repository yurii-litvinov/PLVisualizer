namespace PlVisualizer.Api.Dto.Exceptions.ApiExceptions;

public class PLVisualizerApiBadRequestException : PLVisualizerExceptionBase
{
    public override int StatusCode { get; set; } = 400;

    protected PLVisualizerApiBadRequestException(string message) : base(message)
    {
        
    }

    protected PLVisualizerApiBadRequestException()
    {
        
    }
    
}