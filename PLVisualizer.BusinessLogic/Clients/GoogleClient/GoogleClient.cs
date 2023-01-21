namespace PLVisualizer.BusinessLogic.Clients.GoogleClient;

using DocUtils;
using System.Text.RegularExpressions;
using PlVisualizer.Api.Dto.Exceptions.SpreadsheetsExceptions;
using PlVisualizer.Api.Dto.Tables;

/// <summary>
/// Represents google spreadsheets client
/// </summary>
public class GoogleClient : IGoogleClient
{
    private const string applicationName = "PLVisualizer";
    private GoogleSheetService service;
    private string spreadsheetId;
    private string sheetId;

    private GoogleClient(GoogleSheetService service, string spreadsheetId, string sheetId)
    {
        this.service = service;
        this.spreadsheetId = spreadsheetId;
        this.sheetId = sheetId;
    }

    /// <summary>
    /// Creates a client and asynchronously connects to Google Drive.
    /// </summary>
    /// <param name="spreadsheetId">Id of  Google Spreadsheet.</param>
    /// <param name="sheetId">Title of sheet that contains lecturers.</param>
    public static async Task<GoogleClient> Connect(string spreadsheetId, string sheetId)
        => new GoogleClient(await GoogleSheetService.CreateAsync(
            "client_secret_73783733765-cil43cbdnr1nshgdtao5p0bb6di0git8.apps.googleusercontent.com.json", applicationName),
            spreadsheetId, sheetId);

    public async Task ExportLecturersAsync(Lecturer[] lecturers)
    {
        var range = $"{sheetId}!A:F";

        var sheet = service.Sheet(spreadsheetId, sheetId);

        await ClearSpreadsheet(sheet, range);

        var values = ToValues(lecturers);
        await sheet.WriteSheetAsync("A", "F", values, null);

        var formatTableRequests = GetFormatTableRequests(lecturers);
        await sheet.MergeCellsAsync(formatTableRequests);
    }

    public async Task<Lecturer[]> GetLecturersAsync()
    {
        var values = await service.Sheet(spreadsheetId, sheetId).ReadSheetAsync("A", "F", 2, null);
        return ToLecturerModels(values);
    }

    public async Task<ConfigTableRow[]> GetConfigTableRowsAsync()
    {
        var values = await service.Sheet(spreadsheetId, sheetId).ReadSheetAsync("A", "C", 2, null);
        return await Task.FromResult(ToConfigModel(values));
    }

    private static IList<IList<string>> ToValues(Lecturer[] lecturers)
    {
        var values = new List<IList<string>> 
            { new List<string>() {"ФИО", "Должность", "Процент ставки", "Дисциплины", "Распределенная нагрузка", "Норматив"} };
        foreach (var lecturer in lecturers)
        {
            if (lecturer.Disciplines.Count == 0)
            {
                values.Add(new List<string>() 
                    {lecturer.Name, lecturer.Position, lecturer.FullTimePercent.ToString(), string.Empty, lecturer.DistributedLoad.ToString(), lecturer.RequiredLoad.ToString()});
            }
            values.AddRange(lecturer.Disciplines.Select(discipline => new List<string>
            {
                lecturer.Name, lecturer.Position, lecturer.FullTimePercent.ToString(), discipline.Content, lecturer.DistributedLoad.ToString(), lecturer.RequiredLoad.ToString()
            }));
        }

        return values;
    }

    private static ConfigTableRow[] ToConfigModel(IEnumerable<IEnumerable<object>> configTableRows)
        => configTableRows
            .Select(configTableRow => configTableRow.ToList())
            .Select(configTableRow => new ConfigTableRow( 
                LecturerName: configTableRow[0].ToString() ?? string.Empty,
                Position: configTableRow[1].ToString() ?? string.Empty,
                FullTimePercent: int.Parse(configTableRow[2].ToString() ?? throw new SpreadsheetParsingException(
                    "An error occured while parsing. Ensure Google Spreadsheet has valid format."))))
            .ToArray();
    
    private static Lecturer[] ToLecturerModels(IEnumerable<IEnumerable<string>> values)
    {
        if (values.First().Count() != 6)
        {
            throw new SpreadsheetParsingException(
                "An error occured while parsing. Ensure Google Spreadsheet has valid format.");
        }

        var valuesAsList = values.Select(str => str.ToList()).ToList();
        // otherwise response from google will contain 4 elements in a row
        const int lecturerHeaderCount = 6;
        var models = new List<Lecturer>();
        var disciplines = new List<Discipline>();
        var lecturer = new Lecturer();
        for (var i = 0; i < valuesAsList.Count; i++)
        {
            // iterating through the same lecturer disciplines
            if (valuesAsList[i].Count != lecturerHeaderCount)
            { 
                var disciplineContent = valuesAsList[i][3] ?? string.Empty;
                var discipline = CreateDiscipline(disciplineContent);
                disciplines.Add(discipline);
            }
            else
            {
                lecturer.Name = valuesAsList[i][0] ?? string.Empty;
                lecturer.Position = valuesAsList[i][1] ?? string.Empty;
                lecturer.FullTimePercent = int.Parse(valuesAsList[i][2] ?? throw new SpreadsheetParsingException(
                    "An error occured while parsing. Ensure Google Spreadsheet has valid format."));
                if (valuesAsList[i][3] != string.Empty)
                {
                    disciplines.Add(CreateDiscipline(valuesAsList[i][3] ?? string.Empty));
                }
                lecturer.DistributedLoad = int.Parse(valuesAsList[i][4] ?? throw new SpreadsheetParsingException(
                    "An error occured while parsing. Ensure Google Spreadsheet has valid format."));
                lecturer.RequiredLoad = int.Parse(valuesAsList[i][5].ToString() ??throw new SpreadsheetParsingException(
                    "An error occured while parsing. Ensure Google Spreadsheet has valid format."));
            }
            if (i == valuesAsList.Count - 1 || valuesAsList[i + 1].Count == lecturerHeaderCount)
            {
                lecturer.Disciplines = disciplines;
                models.Add(lecturer);
                disciplines = new List<Discipline>();
                lecturer = new Lecturer();
            }
        }

        return models.ToArray();
    }

    private static List<(int, int, int, int)> GetFormatTableRequests(Lecturer[] lecturers)
    {
        var requests = new List<(int, int, int, int)>();
        const int disciplinesColumnIndex = 3;
        const int columnsCount = 6;
        var currentDisciplinesCount = 0;
        foreach (var lecturer in lecturers)
        {
            var mergeLeftColumns = (currentDisciplinesCount + 1,
                    currentDisciplinesCount + lecturer.Disciplines.Count + 1,
                    0, disciplinesColumnIndex);
            var mergeRightColumns = (currentDisciplinesCount + 1,
                    currentDisciplinesCount + lecturer.Disciplines.Count + 1,
                    disciplinesColumnIndex + 1, columnsCount);
            requests.Add(mergeLeftColumns);
            requests.Add(mergeRightColumns);

            currentDisciplinesCount += lecturer.Disciplines.Count;
        }
        
        return requests;
    }

    // TODO: Should not work.
    private static Discipline CreateDiscipline(string content)
    {
        // take content in [ ]
        var pattern = @"(?<=\[).+?(?=\])";
        var matches = Regex.Matches(content, pattern);
        if (matches == null || matches.Count < 3)
        {
            throw new SpreadsheetParsingException(
                "An error occured while parsing. Ensure Google Spreadsheet has valid format.");
        }
        var term = int.Parse(matches[0].Value);
        var contactLoad = int.Parse(matches[^1].Value);
        var curriculum = matches[^2].Value;

        return new Discipline(
            Id: Guid.NewGuid(),
            Term: "",
            Content: content,
            Code: content[..content.IndexOf(' ')],
            Name: "",
            Load: contactLoad,
            WorkType: matches.Count == 4 ? matches[1].Value : string.Empty,
            Audience: ""
        );
    }

    private static async Task ClearSpreadsheet(Sheet sheet, string range)
    {
        await sheet.ClearRangeAsync(range);
        await sheet.UnmergeCellsAsync();
    }
}