using DocumentFormat.OpenXml.Packaging;
using PlVisualizer.Api.Dto.Tables;
using Microsoft.AspNetCore.Http;

namespace PLVisualizer.BusinessLogic.Services;

public interface ITablesService
{
    Task<Lecturer[]> GetLecturersViaLecturersTableAsync(string spreadsheetId, string sheetTitle = "Лист1");
    Task ExportLecturersAsync(string spreadsheetId,  Lecturer[] lecturers, string sheetTitle = "Лист1");
    Task<Lecturer[]> GetLecturersViaConfigAsync(string spreadsheetId, 
        SpreadsheetDocument spreadsheetDocument, string sheetTitle = "Лист1");
}