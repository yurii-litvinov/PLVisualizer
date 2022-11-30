namespace PlVisualizer.Api.Dto.Exceptions.ApiExceptions;

public class PLVisualizerApiNotFoundException : PLVisualizerExceptionBase
{
    public override int StatusCode { get; set; } = 404;

    protected PLVisualizerApiNotFoundException(string message) : base(message)
    {
        
    }

    protected PLVisualizerApiNotFoundException()
    {
        
    }
}