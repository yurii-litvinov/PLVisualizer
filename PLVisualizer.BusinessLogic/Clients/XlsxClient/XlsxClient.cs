using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Http;
using PlVisualizer.Api.Dto.Tables;
using Sheet = DocumentFormat.OpenXml.Spreadsheet.Sheet;

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
        using var stream = new FileStream(path, FileMode.Open);
        using var spreadsheetDocument = SpreadsheetDocument.Open(stream, false);
        var xlsxTableRows = new XlsxTableRow[]{};
        try
        {
            xlsxTableRows = GetTableRowsFromSpreadsheetDoc(spreadsheetDocument);
        }
        catch (InvalidDataException)
        {
            stream.Dispose();
            spreadsheetDocument.Dispose();
        }

        return xlsxTableRows;
    }

    private XlsxTableRow[] GetTableRowsFromSpreadsheetDoc(SpreadsheetDocument spreadsheetDocument)
    {
        var tableRows = new List<XlsxTableRow>();

        var (workbookPart, sheetData) = OpenXlsxSheet(spreadsheetDocument);
        
        var rows = sheetData.Elements<Row>();
       
        
        tableRows.AddRange(rows.Skip(1).Select(row => row.Elements<Cell>().ToArray())
            .Where(cells => cells.Length > 0)
            .Select(cells => new XlsxTableRow
            {
                Term = GetCellValue(cells[0], workbookPart),
                Subdivision = GetCellValue(cells[1], workbookPart),
                PedagogicalTask = GetCellValue(cells[2], workbookPart),
                DisciplineName = GetCellValue(cells[3], workbookPart),
                WorkType = GetCellValue(cells[4], workbookPart),
                Lecturer = GetCellValue(cells[5], workbookPart),
                SAPSubdivision2 = GetCellValue(cells[6], workbookPart),
                SAPSubdivision1 = GetCellValue(cells[7], workbookPart),
                EducationalProgram = GetCellValue(cells[8], workbookPart)
            }));

        return tableRows.ToArray();
    }

    private static string GetCellValue(Cell cell, WorkbookPart workbookPart)
    {
        var sstPart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
        var sst = sstPart.SharedStringTable;
        if (cell.DataType == null || cell.CellValue == null) return string.Empty;
        if (cell.DataType == CellValues.SharedString)
        {
            var ssid = int.Parse(cell.CellValue.Text);
            return sst.ChildElements[ssid].InnerText;
        }
        return cell.CellValue.InnerText;
    }

    private static (WorkbookPart, SheetData) OpenXlsxSheet(SpreadsheetDocument spreadsheetDocument)
    {
        var workbookPart = spreadsheetDocument.WorkbookPart;
        var sheet = (Sheet)workbookPart.Workbook.Sheets.FirstOrDefault();
        var sheetId = sheet.Id.Value;
        var worksheet = ((WorksheetPart)workbookPart.GetPartById(sheetId)).Worksheet;
        var sheetData = worksheet.Elements<SheetData>().FirstOrDefault();
        return (workbookPart, sheetData);
    }
}