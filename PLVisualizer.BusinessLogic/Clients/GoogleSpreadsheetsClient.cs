using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Vml.Office;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients;
using Google.Apis.Sheets.v4;

public class GoogleSpreadsheetsClient
{
    private static readonly string[] scopes = { SheetsService.Scope.Spreadsheets };
    private string sheetId = "1fsCQvoWo0WidGfZI8rLJqXVHWRULDViCbCd6S5cJ2vE";
    private readonly string sheetTitle = "VisualizerTest";
    private readonly SheetsService service;

    public GoogleSpreadsheetsClient()
    {
        service = new SheetsService(new BaseClientService.Initializer());
    }

    public void SetSheetId(string sheetId)
    {
        this.sheetId = sheetId;
    }
    
    public async Task ExportLecturers(Lecturer[] lecturers)
    {
        var range = $"{sheetTitle}!A:F";
        var valueRange = new ValueRange();
        var values = ToValues(lecturers);
        var appendRequest = service.Spreadsheets.Values.Append(valueRange, sheetId, range);
        // var mergeRequest = service.Spreadsheets.Values.BatchUpdate(new BatchUpdateValuesRequest()., sheetId)
        appendRequest.ValueInputOption =
            SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        await appendRequest.ExecuteAsync();
    }

    public async Task<Lecturer[]> GetLecturers(string spreadsheetId)
    {
        var range = $"{sheetTitle}!A:F";
        var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
        var response = await request.ExecuteAsync();
        var values = response.Values;
        return ToModel(values);
    }

    private  IList<IList<object>> ToValues(Lecturer[] lecturers)
    {
        var values = new List<IList<object>>();
        foreach (var lecturer in lecturers)
        {
            values.AddRange(lecturer.DisciplineIds.Select(disciplineId => new List<object>
            {
                lecturer.Name,
                lecturer.Post,
                lecturer.InterestRate,
                disciplineId,
                lecturer.DistributedLoad,
                lecturer.Standard
            }));
        }

        return values;
    }

    private Lecturer[] ToModel(IList<IList<object>> lecturers)
    {
        var models = new List<Lecturer>();
        var previousLecturer = lecturers[0];
        var disciplineIds = new List<string>();
        foreach (var lecturer in lecturers)
        {
            //iterating through one lecturer
            if (lecturer[0].ToString() == previousLecturer[0].ToString())
            {
                disciplineIds.Add(lecturer[4].ToString() ??
                                  throw new InvalidOperationException("cell with discipline is empty"));
                continue;
            }
            models.Add(new Lecturer
            {
                Name = previousLecturer[0].ToString() ?? string.Empty,
                Post = previousLecturer[1].ToString() ?? string.Empty,
                InterestRate = int.Parse(previousLecturer[2].ToString() ?? string.Empty),
                DisciplineIds = disciplineIds.ToArray(),
                DistributedLoad = int.Parse(previousLecturer[4].ToString() ?? string.Empty),
                Standard = int.Parse(previousLecturer[5].ToString() ?? string.Empty)
            });
            previousLecturer = lecturer;
        }

        return models.ToArray();
    }
}