using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.ExcelClient;

public interface IExcelClient
{
    ExcelTableRow[] GetTableRows(SpreadsheetDocument spreadsheetDocument);
}