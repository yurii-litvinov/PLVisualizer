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
        var lecturers = GetLecturersWithUnmergedDisciplines(tableRows);
        MergeLecturersDisciplines(lecturers);
        return lecturers;
    }

    private static Dictionary<string, Lecturer> GetLecturersWithUnmergedDisciplines(IEnumerable<ExcelTableRow> tableRows)
    {
        var lecturers = new Dictionary<string, Lecturer>();

        foreach (ExcelTableRow row in tableRows)
        {
            var lecturer = row.Lecturer.Split(' ')[0];
            var discipline = CreateDiscipline(row);

            if (lecturers.ContainsKey(lecturer))
            {
                lecturers[lecturer].Disciplines.Add(discipline);
            }
            else
            {
                lecturers.Add(lecturer, new Lecturer
                {
                    Name = lecturer,
                    Disciplines = new List<Discipline> { discipline }
                });
            }
        }

        return lecturers;
    }

    /// <summary>
    /// Merges disciplines into disciplines with common work types.
    /// </summary>
    /// <param name="lecturers">Lecturers with unmerged disciplines.</param>
    /// <exception cref="InvalidDisciplineWorkTypesCountException">Throws when practical disciplines can not be grouped into equal groups.</exception>
    private static void MergeLecturersDisciplines(Dictionary<string, Lecturer> lecturers)
    {
        var updatedLecturers = new List<(string, Lecturer)>();
        foreach (var (key, lecturer) in lecturers)
        {
            var mergedDisciplines = new List<Discipline>();
            var disciplinesGroupedByCode = lecturer.Disciplines.GroupBy(discipline => discipline.Code);
            foreach (var disciplineGroupedByCode in disciplinesGroupedByCode)
            {
                var disciplinesGroupedByTerm = disciplineGroupedByCode.GroupBy(discipline => discipline.Term);
                foreach (var disciplineGroupedByTerm in disciplinesGroupedByTerm)
                {
                    var disciplinesInAGroup = disciplineGroupedByTerm.ToList();
                    var lectureDisciplines = disciplineGroupedByTerm.Where(discipline =>
                        CanBeClassifiedAsLecture(disciplinesInAGroup, discipline.GeneralWorkType)).ToArray();
                    if (lectureDisciplines.Length != 0)
                    {
                        var mainLectureDiscipline = lectureDisciplines.First();
                        var lectureLoad = lectureDisciplines.Sum(lectureDiscipline => lectureDiscipline.TotalLoad);
                        var loadDetails = lectureDisciplines.Select(lectureDiscipline => new LoadDetail(lectureDiscipline.GeneralWorkType, lectureDiscipline.TotalLoad, lectureDiscipline.Audience));

                        mergedDisciplines.Add(
                            new Discipline(
                                Id: Guid.NewGuid(),
                                TotalLoad: lectureLoad,
                                Term: mainLectureDiscipline.Term,
                                Code: mainLectureDiscipline.Code,
                                Name: mainLectureDiscipline.Name,
                                Audience: mainLectureDiscipline.Audience,
                                GeneralWorkType: "Лекции",
                                LoadDetails: loadDetails
                            ));
                    }

                    var nonLectureDisciplines = disciplineGroupedByTerm.Where(discipline => 
                         !CanBeClassifiedAsLecture(disciplinesInAGroup, discipline.GeneralWorkType)).ToArray();
                    if (nonLectureDisciplines.Length == 0)
                    {
                        continue;
                    }

                    var groupedByWorkTypeDisciplines = nonLectureDisciplines.GroupBy(d => d.GeneralWorkType).ToArray();
                    var firstGroupCount = groupedByWorkTypeDisciplines.First().Count();

                    //if (groupedByWorkTypeDisciplines.Any(groupedByWorkTypeDiscipline =>
                    //        groupedByWorkTypeDiscipline.Count() != firstGroupCount 
                    //        && groupedByWorkTypeDiscipline.Key.ToLower() != "лекции"
                    //        && groupedByWorkTypeDiscipline.Key.ToLower() != "контрольные работы"))
                    //{
                    //    throw new InvalidDisciplineWorkTypesCountException(
                    //        $"Discipline {groupedByCodeDiscipline.Key} in semester {groupedByTermDiscipline.Key} does not form equal groups.");
                    //}

                    var mainPracticeDiscipline = nonLectureDisciplines.First();
                    var practiceLoad = groupedByWorkTypeDisciplines.Sum(group => group.First().TotalLoad);

                    for (var i = 0; i < firstGroupCount; i++)
                    {
                        var loadDetails = nonLectureDisciplines.Select(nonLectureDiscipline => new LoadDetail(nonLectureDiscipline.GeneralWorkType, nonLectureDiscipline.TotalLoad, nonLectureDiscipline.Audience));

                        mergedDisciplines.Add(
                            new Discipline(
                                Id: Guid.NewGuid(),
                                TotalLoad: practiceLoad,
                                Term: mainPracticeDiscipline.Term,
                                Code: mainPracticeDiscipline.Code,
                                Name: mainPracticeDiscipline.Name,
                                Audience: mainPracticeDiscipline.Audience,
                                GeneralWorkType: GetGeneralizedPracticeWorkType(nonLectureDisciplines),
                                LoadDetails: loadDetails
                            ));
                    }
                }
            }

            updatedLecturers.Add((key, lecturer with { Disciplines = mergedDisciplines }));
        }

        updatedLecturers.ForEach(pair => lecturers[pair.Item1] = pair.Item2);
    }

    private static Discipline CreateDiscipline(ExcelTableRow tableRow)
        => new Discipline(
            Id: Guid.NewGuid(),
            TotalLoad: tableRow.Hours,
            Term: tableRow.Term,
            Code: tableRow.DisciplineCode,
            Name: tableRow.DisciplineName,
            Audience: tableRow.Audience,
            GeneralWorkType: tableRow.WorkType,
            LoadDetails: new List<LoadDetail>()
        );

    private static bool CanBeClassifiedAsLecture(IEnumerable<Discipline> group, string workType)
    {
        bool IsPartOfLectureCourseWithPassedNotPassedAttestation(string workType)
            => workType is "лекции" or "коллоквиумы" or "промежуточная аттестация (зач)" or "консультации";

        var workTypes = group.Select(x => x.GeneralWorkType.ToLower());
        workType = workType.ToLower();

        if (workType is "лекции" or "коллоквиумы" or "промежуточная аттестация (экз)" or "консультации")
        {
            return true;
        }

        return workType is "промежуточная аттестация (зач)" && workTypes.All(IsPartOfLectureCourseWithPassedNotPassedAttestation);
    }

    private static string GetGeneralizedPracticeWorkType(IEnumerable<Discipline> group)
    {
        var workTypes = group.Select(x => x.GeneralWorkType.ToLower());
        if (workTypes.Contains("практики"))
        {
            return "практики";
        }

        if (workTypes.Contains("семинары"))
        {
            return "семинары";
        }

        return workTypes.First();
    }
}