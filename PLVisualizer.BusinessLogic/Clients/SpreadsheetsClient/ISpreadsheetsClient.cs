using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.SpreadsheetsClient;

public interface ISpreadsheetsClient
{
    void SetSheetTitle(string sheetTitle);
    Task ExportLecturersAsync(Lecturer[] lecturers);
    Task<Lecturer[]> GetLecturersAsync(string spreadsheetId);
    Task<ConfigTableRow[]> GetConfigTableRowsAsync(string spreadsheetId);
}