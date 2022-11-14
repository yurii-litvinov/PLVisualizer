using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.SpreadsheetsClient;

/// <summary>
/// Represents wrapper of Google.Sheets.Api.V4
/// </summary>
public interface ISpreadsheetsClient
{
    Task ExportLecturersAsync(string spreadsheetId, Lecturer[] lecturers, string sheetTitle);
    Task<Lecturer[]> GetLecturersAsync(string spreadsheetId, string sheetTitle);
    Task<ConfigTableRow[]> GetConfigTableRowsAsync(string spreadsheetId, string sheetTitle);
}