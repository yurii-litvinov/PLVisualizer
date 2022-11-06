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


    public async Task<Lecturer[]> GetLecturersViaLecturersTableAsync(string spreadsheetId)
    {
        var lecturersWithoutTerms = await spreadsheetsClient.GetLecturersAsync(spreadsheetId);
        docxClient.FillDisciplinesTerms(lecturersWithoutTerms);
        return lecturersWithoutTerms;
    }

    public async Task ExportLecturersAsync(string spreadsheetId, Lecturer[] lecturers)
    {
        await spreadsheetsClient.ExportLecturersAsync(lecturers);
    }



    public async Task UploadFile(IFormFile file)
    {
        xlsxClient.SetFile(file);
    }

    public async Task<Lecturer[]> GetLecturersViaConfigAsync(string spreadsheetId)
    {
        var xlsxTableRows = xlsxClient.GetTableRows();
        var configTableRows = await spreadsheetsClient.GetConfigTableRowsAsync(spreadsheetId);
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