using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.SpreadsheetsClient;

public interface ISpreadsheetsClient
{
    Task ExportLecturersAsync(string spreadsheetId, Lecturer[] lecturers, string sheetTitle = "Лист1");
    Task<Lecturer[]> GetLecturersAsync(string spreadsheetId, string sheetTitle = "Лист1");
    Task<ConfigTableRow[]> GetConfigTableRowsAsync(string spreadsheetId, string sheetTitle = "Лист1");
}