using System.Text.RegularExpressions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using PlVisualizer.Api.Dto.Exceptions.SpreadsheetsExceptions;
using PlVisualizer.Api.Dto.Tables;
using Google.Apis.Sheets.v4;

namespace PLVisualizer.BusinessLogic.Clients.GoogleClient;

/// <summary>
/// Represents google spreadsheets client
/// </summary>
public class GoogleClient : IGoogleClient
{
    private readonly string applicationName = "PLVisualizer";
    private readonly string[] scopes = { SheetsService.Scope.Spreadsheets };
    private readonly GoogleCredential credential;
    private readonly SheetsService service;

    public GoogleClient()
    {
        credential = GoogleCredential.GetApplicationDefault().CreateScoped(scopes);
        service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            }
        );
    }

    public async Task ExportLecturersAsync(string spreadsheetId, Lecturer[] lecturers, string sheetTitle)
    {
        var range = $"{sheetTitle}!A:F";
        
        var spreadsheet = await service.Spreadsheets.Get(spreadsheetId).ExecuteAsync();
        if (spreadsheet == null)
        {
            throw new SpreadsheetNotFoundException();
        }
        
        var sheet = spreadsheet.Sheets.FirstOrDefault(sheet => sheet.Properties.Title == sheetTitle);
        if (sheet == null)
        {
            throw new SheetNotFoundException() ;
        }
        var sheetId = sheet.Properties.SheetId;
        
        await ClearSpreadsheet(spreadsheetId, sheetId, range);

        var valueRange = new ValueRange();
        var values = ToValues(lecturers);
        valueRange.Values = values;
        var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
        appendRequest.ValueInputOption =
            SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        await appendRequest.ExecuteAsync();

        var formatTableRequests = GetFormatTableRequests(lecturers, sheetId);
        var formatRequest = service.Spreadsheets.BatchUpdate(new BatchUpdateSpreadsheetRequest
        {
            Requests = formatTableRequests
        }, spreadsheetId);
        await formatRequest.ExecuteAsync();
    }

    public async Task<Lecturer[]> GetLecturersAsync(string spreadsheetId, string sheetTitle)
    {
        var spreadsheet = await service.Spreadsheets.Get(spreadsheetId).ExecuteAsync();
        if (spreadsheet == null)
        {
            throw new SpreadsheetNotFoundException();
        }
        var range = $"{sheetTitle}!A:F";
        var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
        var response = await request.ExecuteAsync();
        var values = response.Values.Skip(1).ToArray();
        return ToLecturerModels(values);
    }

    public async Task<ConfigTableRow[]> GetConfigTableRowsAsync(string spreadsheetId, string sheetTitle)
    {
        var spreadsheet = await service.Spreadsheets.Get(spreadsheetId).ExecuteAsync();
        if (spreadsheet == null)
        {
            throw new SpreadsheetNotFoundException();
        }
        var range = $"{sheetTitle}!A:C";
        var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
        var response = await request.ExecuteAsync();
        var values = response.Values.Skip(1).ToArray();
        return ToConfigModel(values);
    }

    private static  IList<IList<object>> ToValues(Lecturer[] lecturers)
    {
        var values = new List<IList<object>> 
            { new List<object>() {"ФИО", "Должность", "Процент ставки", "Дисциплины", "Распределенная нагрузка", "Норматив"} };
        foreach (var lecturer in lecturers)
        {
            if (lecturer.Disciplines.Count == 0)
            {
                values.Add(new List<object>() 
                    {lecturer.Name, lecturer.Post, lecturer.InterestRate, string.Empty, lecturer.DistributedLoad, lecturer.Standard});
            }
            values.AddRange(lecturer.Disciplines.Select(discipline => new List<object>
            {
                lecturer.Name, lecturer.Post, lecturer.InterestRate, discipline.Content, lecturer.DistributedLoad, lecturer.Standard
            }));
        }

        return values;
    }

    private static ConfigTableRow[] ToConfigModel(IEnumerable<IList<object>> configTableRows)
    {
        return configTableRows.Select(configTableRow => new ConfigTableRow { 
                LecturerName = configTableRow[0].ToString() ?? string.Empty,
                Post = configTableRow[1].ToString() ?? string.Empty,
                InterestRate = int.Parse(configTableRow[2].ToString() ?? string.Empty) })
            .ToArray();
    }
    
    private  static Lecturer[] ToLecturerModels(IList<IList<object>> values)
    {
        // otherwise response from google will contain 4 elements in a row
        const int lecturerHeaderCount = 6;
        var models = new List<Lecturer>();
        var disciplines = new List<Discipline>();
        var lecturer = new Lecturer();
        for(var i = 0; i < values.Count; i++)
        {
            //iterating through the same lecturer disciplines
            if (values[i].Count != lecturerHeaderCount)
            { 
                var disciplineContent = values[i][3].ToString() ?? string.Empty;
                var discipline = CreateDiscipline(disciplineContent);
                disciplines.Add(discipline);
            }
            else
            {
                lecturer.Name = values[i][0].ToString() ?? string.Empty;
                lecturer.Post = values[i][1].ToString() ?? string.Empty;
                lecturer.InterestRate = int.Parse(values[i][2].ToString() ?? string.Empty);
                if (values[i][3].ToString() != string.Empty)
                {
                    disciplines.Add(CreateDiscipline(values[i][3].ToString() ?? string.Empty));
                }
                lecturer.DistributedLoad = int.Parse(values[i][4].ToString() ?? string.Empty);
                lecturer.Standard = int.Parse(values[i][5].ToString() ?? string.Empty);
            }
            if (i == values.Count - 1 || values[i + 1].Count == lecturerHeaderCount)
            {
                lecturer.Disciplines = disciplines;
                models.Add(lecturer);
                disciplines = new List<Discipline>();
                lecturer = new Lecturer();
            }
        }

        return models.ToArray();
    }

    private static List<Request> GetFormatTableRequests(Lecturer[] lecturers, int? sheetId)
    {
        var requests = new List<Request>();
        const int disciplinesColumnIndex = 3;
        const int columnsCount = 6;
        var currentDisciplinesCount = 0;
        foreach (var lecturer in lecturers)
        {
            var mergeLeftColumns = new Request
            {
                MergeCells = GetMergeCellsRequest(sheetId, startRowIndex: currentDisciplinesCount + 1,
                    endRowIndex: currentDisciplinesCount + lecturer.Disciplines.Count + 1,
                    startColumnIndex: 0, endColumnIndex: disciplinesColumnIndex)
            };
            var mergeRightColumns = new Request
            {
                MergeCells = GetMergeCellsRequest(sheetId, startRowIndex: currentDisciplinesCount + 1,
                    endRowIndex: currentDisciplinesCount + lecturer.Disciplines.Count + 1,
                    startColumnIndex: disciplinesColumnIndex + 1, endColumnIndex: columnsCount)
            };
            requests.Add(mergeLeftColumns);
            requests.Add(mergeRightColumns);

            currentDisciplinesCount += lecturer.Disciplines.Count;
        }
        
        return requests;
    }

    private static Request GetUnmergeCellsRequest(int? spreadsheetId)
    {
        return new Request()
        {
            UnmergeCells = new UnmergeCellsRequest
            {
                Range = new GridRange
                {
                    SheetId = spreadsheetId
                }
            }
        };
    }
    private static MergeCellsRequest GetMergeCellsRequest(int? spreadsheetId, int startRowIndex, int endRowIndex,
        int startColumnIndex, int endColumnIndex)
    {
        return new MergeCellsRequest
        {
            Range = new GridRange
            {
                SheetId = spreadsheetId,
                StartRowIndex = startRowIndex,
                EndRowIndex = endRowIndex,
                StartColumnIndex = startColumnIndex,
                EndColumnIndex = endColumnIndex
            },
            MergeType = "MERGE_COLUMNS"
        };
    }
    
    private static Discipline CreateDiscipline(string content)
    {
        // take content in [ ]
        var pattern = @"(?<=\[).+?(?=\])";
        var matches = Regex.Matches(content, pattern);
        var term = int.Parse(matches[0].Value);
        var contactLoad = int.Parse(matches[^1].Value);
        var curriculum = matches[^2].Value;

        return new Discipline
        {
            Id = Guid.NewGuid(),
            Term = term,
            Content = content,
            Code = content[..content.IndexOf(' ')],
            ContactLoad = contactLoad,
            EducationalProgram = curriculum,
            WorkType = matches.Count == 4 ? matches[1].Value : string.Empty
        };
    }

    private  async Task ClearSpreadsheet(string spreadsheetId, int? sheetId, string range)
    {
        await service.Spreadsheets.Values
            .Clear(new ClearValuesRequest(), spreadsheetId, range)
            .ExecuteAsync();
        
        var unmergeRequest = GetUnmergeCellsRequest(sheetId);
        await service.Spreadsheets.BatchUpdate(new BatchUpdateSpreadsheetRequest()
                { Requests = new List<Request> { unmergeRequest } }, spreadsheetId)
            .ExecuteAsync();
    }
}