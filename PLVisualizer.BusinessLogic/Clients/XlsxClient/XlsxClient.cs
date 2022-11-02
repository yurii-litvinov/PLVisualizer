using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.XlsxClient;

public class XlsxClient : IXlsxClient
{
    public XlsxTableRow[] TableRows { get; set; }
    
    public void SetFile(IFormFile file)
    {
        var fileStream = file.OpenReadStream();
        fileStream.Position = 0;
        using var document = SpreadsheetDocument.Open(fileStream, false);
        var workbookPart = document.WorkbookPart;
        var workbook = workbookPart.Workbook;
        var sheets = workbook.Descendants<Sheet>();
        foreach (var sheet in sheets)
        {
            var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);

            var rows = worksheetPart.Worksheet.Descendants<Row>();
            foreach (var row in rows)
            {
                var cells = row.Descendants<Cell>();
            }
        }
    }
}