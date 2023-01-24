namespace PLVisualizer.BusinessLogic.Services;

using DocumentFormat.OpenXml.Packaging;
using PlVisualizer.Api.Dto.Exceptions.DocxExceptions;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.ExcelClient;
using PLVisualizer.BusinessLogic.Clients.GoogleClient;
using PLVisualizer.BusinessLogic.Extensions;
using System.Linq;

/// <summary>
/// Tables service
/// </summary>
public class TablesService : ITablesService
{
    private IExcelClient excelClient;

    public TablesService(IExcelClient excelClient)
    {
        this.excelClient = excelClient;
    }

    public async Task<Lecturer[]> GetLecturersViaLecturersTableAsync(string spreadsheetId, string sheetTitle)
    {
        var googleClient = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        return await googleClient.GetLecturersAsync();
    }

    public async Task ExportLecturersAsync(string spreadsheetId, Lecturer[] lecturers, string sheetTitle)
    {
        var googleClient = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        await googleClient.ExportLecturersAsync(lecturers);
    }

    public async Task<Lecturer[]> GetLecturersViaConfigAsync(string spreadsheetId,
        SpreadsheetDocument spreadsheetDocument,
        string sheetTitle)
    {
        var xlsxTableRows = excelClient.GetTableRows(spreadsheetDocument);

        var googleClient = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        var configTableRows = await googleClient.GetConfigTableRowsAsync();

        return GetLecturersWithDisciplines(xlsxTableRows)
            .WithConfigInformation(configTableRows)
            .WithStandards()
            .WithDistributedLoad()
            .ToArray();
    }

    private Dictionary<string, Lecturer> GetLecturersWithDisciplines(IEnumerable<ExcelTableRow> tableRows)
    {
        var lecturerRowGroups = GroupByLecturers(tableRows);
        var lecturers = CreateDisciplines(lecturerRowGroups);
        return lecturers;
    }

    private static Dictionary<string, List<ExcelTableRow>> GroupByLecturers(IEnumerable<ExcelTableRow> tableRows)
    {
        var lecturerRowGroups = new Dictionary<string, List<ExcelTableRow>>();

        foreach (ExcelTableRow row in tableRows)
        {
            var lecturerKey = row.Lecturer.Split(' ')[0];

            if (!lecturerRowGroups.ContainsKey(lecturerKey))
            {
                lecturerRowGroups.Add(lecturerKey, new());
            }

            lecturerRowGroups[lecturerKey].Add(row);
        }

        return lecturerRowGroups;
    }

    /// <summary>
    /// Merges disciplines into disciplines with common work types.
    /// </summary>
    /// <param name="lecturers">Lecturers with unmerged disciplines.</param>
    /// <exception cref="InvalidDisciplineWorkTypesCountException">Throws when practical disciplines can not be grouped into equal groups.</exception>
    private static Dictionary<string, Lecturer> CreateDisciplines(Dictionary<string, List<ExcelTableRow>> lecturerRowGroups)
    {
        var result = new Dictionary<string, Lecturer>();

        foreach (var (key, rows) in lecturerRowGroups)
        {
            var disciplines = new List<Discipline>();
            var rowsGroupedByCode = rows.GroupBy(row => row.DisciplineCode);

            foreach (var rowGroupedByCode in rowsGroupedByCode)
            {
                var rowsGroupedByTerm = rowGroupedByCode.GroupBy(row => row.Term);
                foreach (var rowGroupedByTerm in rowsGroupedByTerm)
                {
                    var rowsInAGroup = rowGroupedByTerm.ToList();
                    
                    var lectureRows = rowGroupedByTerm.Where(row => CanBeClassifiedAsLecture(rowsInAGroup, row.WorkType));
                    
                    if (lectureRows.Count() != 0)
                    {
                        var mainLectureWorkUnit = lectureRows.First();
                        var lectureLoad = lectureRows.Sum(lectureRow => lectureRow.Hours);
                        var loadDetails = lectureRows.Select(lectureRow => new LoadDetail(lectureRow.WorkType, lectureRow.Hours, lectureRow.Audience));

                        disciplines.Add(
                            new Discipline(
                                Id: Guid.NewGuid(),
                                TotalLoad: lectureLoad,
                                Term: mainLectureWorkUnit.Term,
                                Code: mainLectureWorkUnit.DisciplineCode,
                                Name: mainLectureWorkUnit.DisciplineName,
                                Audience: mainLectureWorkUnit.Audience,
                                GeneralWorkType: "Лекции",
                                LoadDetails: loadDetails
                            ));
                    }

                    var practiceRows = rowGroupedByTerm.Where(row => !CanBeClassifiedAsLecture(rowsInAGroup, row.WorkType));
                    if (practiceRows.Count() == 0)
                    {
                        continue;
                    }

                    var practiceRowsGroupedByWorkType = practiceRows.GroupBy(d => d.WorkType);
                    var firstGroupCount = practiceRowsGroupedByWorkType.First().Count();

                    
                    var practiceLoad = practiceRowsGroupedByWorkType.Sum(group => group.First().Hours);

                    var practiceRowsGroupedByAudience = practiceRows.GroupBy(d => d.Audience);

                    foreach (var practiceGroupedByAudience in practiceRowsGroupedByAudience)
                    {
                        var practicesInAGroup = practiceGroupedByAudience.ToList();
                        var mainPracticeWorkUnit = practicesInAGroup.First();

                        var loadDetails = practicesInAGroup.Select(practiceRow => new LoadDetail(practiceRow.WorkType, practiceRow.Hours, practiceRow.Audience));

                        disciplines.Add(
                            new Discipline(
                                Id: Guid.NewGuid(),
                                TotalLoad: practiceLoad,
                                Term: mainPracticeWorkUnit.Term,
                                Code: mainPracticeWorkUnit.DisciplineCode,
                                Name: mainPracticeWorkUnit.DisciplineName,
                                Audience: mainPracticeWorkUnit.Audience,
                                GeneralWorkType: GetGeneralizedPracticeWorkType(practiceRows),
                                LoadDetails: loadDetails
                            ));
                    }
                }
            }

            result.Add(key, new Lecturer { Name = key, Disciplines = disciplines });
        }

        return result;
    }

    private static bool CanBeClassifiedAsLecture(IEnumerable<ExcelTableRow> group, string workType)
    {
        bool IsDefinitelyPartOfLectureCourse(string workType)
            => workType is "лекции" or "коллоквиумы" or "промежуточная аттестация (экз)" or "консультации";

        bool CanBePartOfLectureCourse(string workType)
            => IsDefinitelyPartOfLectureCourse(workType) || workType is "промежуточная аттестация (зач)" or "текущий контроль (ауд)" or "контрольные работы";

        var workTypes = group.Select(x => x.WorkType.ToLower());
        workType = workType.ToLower();

        return IsDefinitelyPartOfLectureCourse(workType) || workTypes.All(CanBePartOfLectureCourse);
    }

    private static string GetGeneralizedPracticeWorkType(IEnumerable<ExcelTableRow> group)
    {
        var workTypes = group.Select(x => x.WorkType.ToLower());
        if (workTypes.Contains("практические занятия"))
        {
            return "практические занятия";
        }

        if (workTypes.Contains("семинары"))
        {
            return "семинары";
        }

        return workTypes.First();
    }
}