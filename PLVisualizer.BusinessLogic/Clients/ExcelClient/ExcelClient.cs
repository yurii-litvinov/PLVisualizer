using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Http;
using PlVisualizer.Api.Dto.Tables;
using Sheet = DocumentFormat.OpenXml.Spreadsheet.Sheet;

namespace PLVisualizer.BusinessLogic.Clients.ExcelClient;

public class ExcelClient : IExcelClient
{
    public ExcelTableRow[] GetTableRows(SpreadsheetDocument spreadsheetDocument)
    {
        var tableRows = new List<ExcelTableRow>();

        var (workbookPart, sheetData) = OpenExcelSheet(spreadsheetDocument);

        var rows = sheetData.Elements<Row>();
        
        tableRows.AddRange(rows.Skip(1).Select(row => row.Elements<Cell>().ToArray())
            .Where(cells => cells.Length > 0 && GetCellValue(cells[0], workbookPart).Length > 0)
            .Select(cells => new ExcelTableRow
            {
                Term = int.Parse(GetCellValue(cells[0], workbookPart).Replace("Семестр", string.Empty).Trim()),
                Subdivision = GetCellValue(cells[1], workbookPart),
                PedagogicalTask = GetCellValue(cells[2], workbookPart),
                DisciplineName = GetCellValue(cells[3], workbookPart),
                WorkType = GetCellValue(cells[4], workbookPart),
                Lecturer = RetrieveLecturer(GetCellValue(cells[5], workbookPart)),
                SAPSubdivision2 = GetCellValue(cells[6], workbookPart),
                SAPSubdivision1 = GetCellValue(cells[7], workbookPart),
                EducationalProgram = GetCellValue(cells[8], workbookPart)
            }));

        return tableRows.ToArray();
    }

    private static string RetrieveLecturer(string cellValue)
    {
        return cellValue[..cellValue.IndexOf(',')];
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

    private static (WorkbookPart, SheetData) OpenExcelSheet(SpreadsheetDocument spreadsheetDocument)
    {
        var workbookPart = spreadsheetDocument.WorkbookPart;
        var sheet = (Sheet)workbookPart.Workbook.Sheets.FirstOrDefault();
        var sheetId = sheet.Id.Value;
        var worksheet = ((WorksheetPart)workbookPart.GetPartById(sheetId)).Worksheet;
        var sheetData = worksheet.Elements<SheetData>().FirstOrDefault();
        return (workbookPart, sheetData);
    }
}