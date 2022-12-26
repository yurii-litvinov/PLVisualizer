using PlVisualizer.Api.Dto.Exceptions.ApiExceptions;

namespace PlVisualizer.Api.Dto.Exceptions.DocxExceptions;

public class WorkingPlanNotFoundException : PLVisualizerApiNotFoundException
{ 
    public WorkingPlanNotFoundException(string message) : base(message){ }
}