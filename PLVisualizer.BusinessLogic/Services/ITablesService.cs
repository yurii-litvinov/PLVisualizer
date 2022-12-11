using DocumentFormat.OpenXml.Packaging;
using PlVisualizer.Api.Dto.Tables;
using Microsoft.AspNetCore.Http;

namespace PLVisualizer.BusinessLogic.Services;

/// <summary>
/// Represents tables service 
/// </summary>
public interface ITablesService
{
    /// <summary>
    /// Gets lecturers from a Google Spreadsheet previously exported by the application
    /// </summary>
    /// <param name="spreadsheetId">Id of Google Spreadsheet</param>
    /// <param name="sheetTitle">Title of sheet that contains lecturers</param>
    Task<Lecturer[]> GetLecturersViaLecturersTableAsync(string spreadsheetId, string sheetTitle = "Лист1");
    
    /// <summary>
    /// Exports lecturers to specified google spreadsheet
    /// </summary>
    /// <param name="spreadsheetId">Id of google spreadsheet</param>
    /// <param name="lecturers">Lecturers to export</param>
    /// <param name="sheetTitle">Title of sheet to export lecturers</param>
    Task ExportLecturersAsync(string spreadsheetId,  Lecturer[] lecturers, string sheetTitle = "Лист1");
    
    /// <summary>
    /// Gets lecturers from SpreadsheetDocument filtered via configuration Google Spreadsheet
    /// </summary>
    /// <param name="spreadsheetId">Id of Google Spreadsheet</param>
    /// <param name="spreadsheetDocument">Xlsx file converted to SpreadsheetDocument</param>
    /// <param name="sheetTitle">Title of configuration sheet</param>
    Task<Lecturer[]> GetLecturersViaConfigAsync(string spreadsheetId, 
        SpreadsheetDocument spreadsheetDocument, string sheetTitle = "Лист1");
}