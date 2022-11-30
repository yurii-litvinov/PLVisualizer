using DocumentFormat.OpenXml.Packaging;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.DocxClient;
using PLVisualizer.BusinessLogic.Clients.ExcelClient;
using PLVisualizer.BusinessLogic.Clients.GoogleClient;
using PLVisualizer.BusinessLogic.Extensions;

namespace PLVisualizer.BusinessLogic.Services;

/// <summary>
/// Tables service
/// </summary>
public class TablesService : ITablesService
{
    private IDocxClient docxClient;
    private IExcelClient excelClient;
    private IGoogleClient googleClient;

    public TablesService(IDocxClient docxClient, IExcelClient excelClient, IGoogleClient googleClient)
    {
        this.docxClient = docxClient;
        this.excelClient = excelClient;
        this.googleClient = googleClient;
    }


    public async Task<Lecturer[]> GetLecturersViaLecturersTableAsync(string spreadsheetId, string sheetTitle)
    {
        return await googleClient.GetLecturersAsync(spreadsheetId, sheetTitle);
    }

    public async Task ExportLecturersAsync(string spreadsheetId, Lecturer[] lecturers, string sheetTitle)
    {
        await googleClient.ExportLecturersAsync(spreadsheetId, lecturers, sheetTitle);
    }
    
    public async Task<Lecturer[]> GetLecturersViaConfigAsync(string spreadsheetId, 
        SpreadsheetDocument spreadsheetDocument,
        string sheetTitle)
    {
        var xlsxTableRows = excelClient.GetTableRows(spreadsheetDocument);
        
        var configTableRows = await googleClient.GetConfigTableRowsAsync(spreadsheetId, sheetTitle);
        
        return docxClient.GetLecturersWithDisciplines(xlsxTableRows)
            .WithConfigInformation(configTableRows)
            .WithStandards()
            .WithDistributedLoad()
            .ToArray();
    }
}