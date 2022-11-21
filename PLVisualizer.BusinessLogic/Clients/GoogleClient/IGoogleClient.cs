using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.GoogleClient;

/// <summary>
/// Represents wrapper of Google.Sheets.Api.V4
/// </summary>
public interface IGoogleClient
{
    Task ExportLecturersAsync(string spreadsheetId, Lecturer[] lecturers, string sheetTitle);
    Task<Lecturer[]> GetLecturersAsync(string spreadsheetId, string sheetTitle);
    Task<ConfigTableRow[]> GetConfigTableRowsAsync(string spreadsheetId, string sheetTitle);
}