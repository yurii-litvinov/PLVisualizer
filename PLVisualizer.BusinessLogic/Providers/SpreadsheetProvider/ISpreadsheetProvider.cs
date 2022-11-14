using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;

namespace PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;

public interface ISpreadsheetProvider
{
    SpreadsheetDocument GetSpreadsheetDocument(string path);
    SpreadsheetDocument GetSpreadsheetDocument(IFormFile file);
}