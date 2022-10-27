using Microsoft.AspNetCore.Http;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients;

namespace PLVisualizer.BusinessLogic.Services;

public class TablesService : ITablesService
{
    private DocxClient docxClient;
    private XlsxClient xlsxClient;
    private GoogleSpreadsheetsClient spreadsheetsClient;

    public TablesService(DocxClient docxClient, XlsxClient xlsxClient, GoogleSpreadsheetsClient spreadsheetsClient)
    {
        this.docxClient = docxClient;
        this.xlsxClient = xlsxClient;
        this.spreadsheetsClient = spreadsheetsClient;
    }
    
    
    public async Task<Lecturer[]> GetLecturers(string spreadsheetId)
    {
        throw new NotImplementedException();
    }

    public async Task UnloadLecturers(string spreadsheetId, Lecturer[] lecturers)
    {
        throw new NotImplementedException();
    }

    public async Task<Discipline[]> GetRequiredDisciplines(string spreadsheetId)
    {
        throw new NotImplementedException();
    }

    public async Task UploadFile(IFormFile file)
    {
        throw new NotImplementedException();
    }
}