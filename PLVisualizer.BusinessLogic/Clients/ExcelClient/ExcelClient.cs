namespace PLVisualizer.BusinessLogic.Clients.ExcelClient;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using PlVisualizer.Api.Dto.Exceptions.SpreadsheetsExceptions;
using PlVisualizer.Api.Dto.Tables;
using Sheet = DocumentFormat.OpenXml.Spreadsheet.Sheet;
using System.Diagnostics;

public class ExcelClient : IExcelClient
{
    public ExcelTableRow[] GetTableRows(SpreadsheetDocument spreadsheetDocument)
    {
        var tableRows = new List<ExcelTableRow>();

        var (workbookPart, sheetData) = OpenExcelSheet(spreadsheetDocument);

        var rows = sheetData.Elements<Row>();

        tableRows.AddRange(rows.Skip(1).Select(row => row.Elements<Cell>().ToArray())
            .Where(cells => cells.Length > 0 && GetCellValue(cells[0], workbookPart).Length > 0)
            .Select(cells => new ExcelTableRow(
                Term: GetCellValue(cells[0], workbookPart),
                DisciplineCode: GetDisciplineCode(GetCellValue(cells[1], workbookPart)),
                DisciplineName: GetDisciplineName(GetCellValue(cells[1], workbookPart)),
                WorkType: GetCellValue(cells[6], workbookPart),
                Lecturer: RetrieveLecturer(GetCellValue(cells[7], workbookPart)),
                Hours: GetHours(GetCellValue(cells[10], workbookPart)),
                Audience: GetCellValue(cells[12], workbookPart)
            )));

        return tableRows.ToArray();
    }

    private static string RetrieveLecturer(string cellValue)
    {
        if (cellValue.IndexOf(',') < 0)
        {
            throw new SpreadsheetParsingException($"Invalid table format at {cellValue}");
        }

        return cellValue[..cellValue.IndexOf(',')];
    }

    private static string GetDisciplineName(string discipline)
        => discipline[7..];

    private static string GetDisciplineCode(string discipline)
        => discipline[0..6];

    private static int GetHours(string hours)
        => int.Parse(hours);

    private static string GetCellValue(Cell cell, WorkbookPart workbookPart)
    {
        var sstPart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
        Debug.Assert(sstPart != null);
        var sst = sstPart.SharedStringTable;
        if (cell.CellValue == null)
        {
            return string.Empty;
        }

        if (cell.DataType != null && cell.DataType == CellValues.SharedString)
        {
            var ssid = int.Parse(cell.CellValue.Text);
            return sst.ChildElements[ssid].InnerText.Trim();
        }

        return cell.CellValue.InnerText.Trim();
    }

    private static (WorkbookPart, SheetData) OpenExcelSheet(SpreadsheetDocument spreadsheetDocument)
    {
        var workbookPart = spreadsheetDocument.WorkbookPart;
        Debug.Assert(workbookPart?.Workbook.Sheets != null);
        var sheet = workbookPart.Workbook.Sheets.FirstOrDefault() as Sheet ?? throw new SpreadsheetParsingException("No sheets found");
        Debug.Assert(sheet.Id != null);
        var sheetId = sheet.Id.Value;
        Debug.Assert(sheetId != null);
        var worksheet = ((WorksheetPart)workbookPart.GetPartById(sheetId)).Worksheet;
        var sheetData = worksheet.Elements<SheetData>().FirstOrDefault() ?? throw new SpreadsheetParsingException("No sheet data found");
        return (workbookPart, sheetData);
    }
}