using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.XlsxClient;

public class XlsxClient : IXlsxClient
{
    public XlsxTableRow[] GetTableRows(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        stream.Position = 0;
        using var spreadsheetDocument = SpreadsheetDocument.Open(stream, false);
        return GetTableRowsFromSpreadsheetDoc(spreadsheetDocument);
    }
    
    public XlsxTableRow[] GetTableRows(string path)
    {
        using var spreadsheetDocument = SpreadsheetDocument.Open(path, false);
        return GetTableRowsFromSpreadsheetDoc(spreadsheetDocument);
    }

    private XlsxTableRow[] GetTableRowsFromSpreadsheetDoc(SpreadsheetDocument spreadsheetDocument)
    {
        var tableRows = new List<XlsxTableRow>();
        var workbookPart = spreadsheetDocument.WorkbookPart;
        var workbook = workbookPart.Workbook;
        var sheets = workbook.Descendants<Sheet>();
        foreach (var sheet in sheets)
        {
            var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);

            var rows = worksheetPart.Worksheet.Elements<Row>();
            tableRows.AddRange(rows.Skip(1).Select(row => row.Elements<Cell>().ToArray())
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
        
        
        return tableRows.ToArray();
    }
}