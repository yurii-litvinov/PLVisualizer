using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;

namespace PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;

/// <summary>
/// Represents SpreadsheetDocument of Open Xml provider
/// </summary>
public interface ISpreadsheetProvider
{
    /// <summary>
    /// Gets SpreadsheetDocument
    /// </summary>
    /// <param name="path">Path to file</param>
    SpreadsheetDocument GetSpreadsheetDocument(string path);
    
    /// <summary>
    /// Gets SpreadsheetDocument 
    /// </summary>
    /// <param name="file">File sent over the network</param>
    SpreadsheetDocument GetSpreadsheetDocument(IFormFile file);
}