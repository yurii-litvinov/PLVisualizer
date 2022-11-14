using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.SpreadsheetsClient;

public interface ISpreadsheetsClient
{
    Task ExportLecturersAsync(string spreadsheetId, Lecturer[] lecturers, string sheetTitle);
    Task<Lecturer[]> GetLecturersAsync(string spreadsheetId, string sheetTitle);
    Task<ConfigTableRow[]> GetConfigTableRowsAsync(string spreadsheetId, string sheetTitle);
}