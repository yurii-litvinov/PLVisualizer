using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;

namespace PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;

public class SpreadsheetProvider : ISpreadsheetProvider
{
    public SpreadsheetDocument GetSpreadsheetDocument(string path)
    {
        return SpreadsheetDocument.Open(path, false);
    }

    public SpreadsheetDocument GetSpreadsheetDocument(IFormFile file)
    {
        var stream = file.OpenReadStream();
        stream.Position = 0;
        return SpreadsheetDocument.Open(stream, false);
    }
}