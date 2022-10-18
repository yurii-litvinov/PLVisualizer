using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using PlVisualizer.Api.Dto;

namespace PLVisualizer.BusinessLogic.Clients;
using Google.Apis.Sheets.v4;

public class GoogleSpreadsheetsClient
{
    private static readonly string[] scopes = { SheetsService.Scope.Spreadsheets };
    private string _sheetId = "1fsCQvoWo0WidGfZI8rLJqXVHWRULDViCbCd6S5cJ2vE";
    private readonly string sheetTitle = "VisualizerTest";
    private readonly SheetsService service;

    public GoogleSpreadsheetsClient(string sheetId)
    {
        service = new SheetsService(new BaseClientService.Initializer());
    }

    public void SetSheetId(string sheetId)
    {
        _sheetId = sheetId;
    }

    public async Task ExportDistributedLoad(Lecturer[] lecturers)
    {
        var range = $"{sheetTitle}!A:F";
        var valueRange = new ValueRange();
        foreach (var lecturer in lecturers)
        {
            var exportRow = new List<object>
            {
                lecturer.Name, lecturer.Post, lecturer.InterestRate,
                lecturer.Disciplines, lecturer.DistributedLoad, lecturer.Standard
            };
            valueRange.Values = new List<IList<object>> { exportRow };

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, _sheetId, range);
            appendRequest.ValueInputOption =
                SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            await appendRequest.ExecuteAsync();
        }
    }

    public async Task<Lecturer[]> ReadAllLecturers()
    {
        throw new NotImplementedException();
    }
}