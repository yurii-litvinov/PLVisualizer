namespace PlVisualizer.Api.Dto.Exceptions;

public abstract class PLVisualizerExceptionBase : Exception
{
    public virtual int StatusCode { get; set; }

    protected PLVisualizerExceptionBase(string message) : base(message)
    {
        
    }

    public PLVisualizerExceptionBase()
    {
        
    }
}