using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.SpreadsheetsClient;

public interface ISpreadsheetsClient
{
    Task ExportLecturers(Lecturer[] lecturers);
    Task<Lecturer[]> GetLecturers(string spreadsheetId);
    Task<ConfigTableRow[]> GetConfigTableRows(string spreadsheetId);
}