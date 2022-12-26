namespace PlVisualizer.Api.Dto.Exceptions.ApiExceptions;

public class PLVisualizerApiNotFoundException : PLVisualizerExceptionBase
{
    public override int StatusCode { get; set; } = 404;

    public PLVisualizerApiNotFoundException(string message) : base(message)
    {
        
    }

    protected PLVisualizerApiNotFoundException()
    {
        
    }
}