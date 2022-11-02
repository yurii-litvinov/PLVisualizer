using PlVisualizer.Api.Dto.Tables;
using Microsoft.AspNetCore.Http;

namespace PLVisualizer.BusinessLogic.Services;

public interface ITablesService
{
    Task<Lecturer[]> GetLecturersViaLecturersTable(string spreadsheetId);
    Task UnloadLecturers(string spreadsheetId, Lecturer[] lecturers);
    Task UploadFile(IFormFile file);
    Task<Lecturer[]> GetLecturersViaConfig(string spreadsheetId);
}