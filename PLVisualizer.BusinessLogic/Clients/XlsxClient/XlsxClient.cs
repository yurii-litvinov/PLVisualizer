using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.XlsxClient;

public class XlsxClient : IXlsxClient
{
    public XlsxTableRow[]? TableRows { get; set; }
    
    public void SetFile(IFormFile file)
    {
        var tableRows = new List<XlsxTableRow>();
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
            tableRows.AddRange(rows.Select(row => row.Descendants<Cell>().ToArray())
            .Select(cells => new XlsxTableRow
            {
                Term = cells[0].CellValue?.Text ?? string.Empty,
                Subdivision = cells[1].CellValue?.Text ?? string.Empty,
                PedagogicalTask = cells[2].CellValue?.Text ?? string.Empty,
                DisciplineName = cells[3].CellValue?.Text ?? string.Empty,
                WorkType = cells[4].CellValue?.Text ?? string.Empty,
                Lecturer = cells[5].CellValue?.Text ?? string.Empty,
                SAPSubdivision2 = cells[6].CellValue?.Text ?? string.Empty,
                SAPSubdivision1 = cells[7].CellValue?.Text ?? string.Empty,
                EducationalProgram = cells[8].CellValue?.Text ?? string.Empty
            }));
        }

        TableRows = tableRows.ToArray();
    }
}