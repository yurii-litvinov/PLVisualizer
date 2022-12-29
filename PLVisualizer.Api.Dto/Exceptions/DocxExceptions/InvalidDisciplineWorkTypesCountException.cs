using PlVisualizer.Api.Dto.Exceptions.ApiExceptions;

namespace PlVisualizer.Api.Dto.Exceptions.DocxExceptions;

public class InvalidDisciplineWorkTypesCountException : PLVisualizerApiBadRequestException
{
    public InvalidDisciplineWorkTypesCountException(string message) : base(message) { }
}