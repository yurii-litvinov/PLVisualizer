using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.XlsxClient;

public interface IXlsxClient
{
    XlsxTableRow[] GetTableRows(SpreadsheetDocument spreadsheetDocument);
}