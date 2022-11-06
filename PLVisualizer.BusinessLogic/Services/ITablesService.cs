using PlVisualizer.Api.Dto.Tables;
using Microsoft.AspNetCore.Http;

namespace PLVisualizer.BusinessLogic.Services;

public interface ITablesService
{
    Task<Lecturer[]> GetLecturersViaLecturersTableAsync(string spreadsheetId);
    Task ExportLecturersAsync(string spreadsheetId, Lecturer[] lecturers);
    Task UploadFile(IFormFile file);
    Task<Lecturer[]> GetLecturersViaConfigAsync(string spreadsheetId);
}