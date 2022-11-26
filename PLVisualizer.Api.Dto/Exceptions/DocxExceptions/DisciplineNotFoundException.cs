using PlVisualizer.Api.Dto.Exceptions.ApiExceptions;

namespace PlVisualizer.Api.Dto.Exceptions.DocxExceptions;

public class DisciplineNotFoundException : PLVisualizerApiNotFoundException
{
    public DisciplineNotFoundException(string message) : base(message)
    {
        
    }

    public DisciplineNotFoundException()
    {
        
    }
}