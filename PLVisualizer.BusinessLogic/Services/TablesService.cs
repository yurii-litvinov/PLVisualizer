using Microsoft.AspNetCore.Http;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients;
using PLVisualizer.BusinessLogic.Clients.DocxClient;
using PLVisualizer.BusinessLogic.Clients.SpreadsheetsClient;
using PLVisualizer.BusinessLogic.Clients.XlsxClient;

namespace PLVisualizer.BusinessLogic.Services;

/// <summary>
/// Tables service
/// </summary>
public class TablesService : ITablesService
{
    private IDocxClient docxClient;
    private IXlsxClient xlsxClient;
    private ISpreadsheetsClient spreadsheetsClient;

    public TablesService(IDocxClient docxClient, IXlsxClient xlsxClient, ISpreadsheetsClient spreadsheetsClient)
    {
        this.docxClient = docxClient;
        this.xlsxClient = xlsxClient;
        this.spreadsheetsClient = spreadsheetsClient;
    }
    
    
    public async Task<Lecturer[]> GetLecturersViaLecturersTable(string spreadsheetId)
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
        xlsxClient.SetFile(file);
    }

    public async Task<Lecturer[]> GetLecturersViaConfig(string spreadsheetId)
    {
        var xlsxTableRows = xlsxClient.TableRows;
        var configTableRows = await spreadsheetsClient.GetConfigTableRows(spreadsheetId);
        var lecturersWithDisciplines = docxClient.GetLecturersWithDisciplines(xlsxTableRows);
        foreach (var configTableRow in configTableRows)
        {
            var lecturer = lecturersWithDisciplines[configTableRow.LecturerName];
            lecturer.Post = configTableRow.Post;
            lecturer.InterestRate = configTableRow.InterestRate;
        }

        return lecturersWithDisciplines.Values.ToArray();
    }
    
}