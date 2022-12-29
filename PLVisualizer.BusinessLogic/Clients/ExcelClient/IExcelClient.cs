using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.ExcelClient;

/// <summary>
/// Represents client which use Open Xml to retrieve info from xlsx file
/// </summary>
public interface IExcelClient
{
    /// <summary>
    /// Gets discipline models from spreadsheet document.
    /// </summary>
    /// <param name="spreadsheetDocument">Spreadsheet document of input xlsx file.</param>
    /// <returns>Models of table rows.</returns>
    ExcelTableRow[] GetTableRows(SpreadsheetDocument spreadsheetDocument);
}