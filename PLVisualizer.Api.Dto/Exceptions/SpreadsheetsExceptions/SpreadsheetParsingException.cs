using PlVisualizer.Api.Dto.Exceptions.ApiExceptions;

namespace PlVisualizer.Api.Dto.Exceptions.SpreadsheetsExceptions;

public class SpreadsheetParsingException : PLVisualizerApiBadRequestException
{
    public SpreadsheetParsingException(string message) : base(message) { }
}