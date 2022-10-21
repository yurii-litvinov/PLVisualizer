using PlVisualizer.Api.Dto;

namespace PLVisualizer.BusinessLogic.Services;

public interface ITablesService
{
    Task<Lecturer[]> GetLecturers(string spreadsheetId);
    Task UnloadLecturers(string spreadsheetId, Lecturer[] lecturers);
    Task<Discipline[]> GetRequiredDisciplines(string spreadsheetId);
}