using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.GoogleClient;

/// <summary>
/// Represents client that is using Google.Sheets.Api.V4 to import and export lecturers.
/// </summary>
public interface IGoogleClient
{
    /// <summary>
    /// Exports lecturers to specified Google Spreadsheet.
    /// </summary>
    /// <param name="spreadsheetId">Id of Google Spreadsheet.</param>
    /// <param name="lecturers">Lecturers to export.</param>
    /// <param name="sheetTitle">Title of sheet to export lecturers.</param>
    Task ExportLecturersAsync(string spreadsheetId, Lecturer[] lecturers, string sheetTitle);
    
    /// <summary>
    /// Gets lecturers from a Google Spreadsheet previously exported by the application.
    /// </summary>
    /// <param name="spreadsheetId">Id of  Google Spreadsheet.</param>
    /// <param name="sheetTitle">Title of sheet that contains lecturers.</param>
    Task<Lecturer[]> GetLecturersAsync(string spreadsheetId, string sheetTitle);
    
    /// <summary>
    /// Gets configuration table row models.
    /// </summary>
    /// <param name="spreadsheetId">Id of Google Spreadsheet.</param>
    /// <param name="sheetTitle">Title of sheet that contains configuration.</param>
    Task<ConfigTableRow[]> GetConfigTableRowsAsync(string spreadsheetId, string sheetTitle);
}