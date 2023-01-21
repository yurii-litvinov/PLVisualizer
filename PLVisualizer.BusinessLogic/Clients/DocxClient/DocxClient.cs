namespace PLVisualizer.BusinessLogic.Clients.DocxClient;

using CurriculumParser;
using PlVisualizer.Api.Dto.Exceptions.ApiExceptions;
using PlVisualizer.Api.Dto.Exceptions.DocxExceptions;
using PlVisualizer.Api.Dto.Tables;
using Discipline = PlVisualizer.Api.Dto.Tables.Discipline;

public class DocxClient : IDocxClient
{
    public Dictionary<string, Lecturer> GetLecturersWithDisciplines(IEnumerable<ExcelTableRow> tableRows)
    {
        var lecturers = GetLecturersWithUnmergedDisciplines(tableRows);
        MergeLecturersDisciplines(lecturers);
        return lecturers;
    }
    
    private static Dictionary<string, Lecturer> GetLecturersWithUnmergedDisciplines(IEnumerable<ExcelTableRow> tableRows)
    {
        var lecturers = new Dictionary<string, Lecturer>();

        //group by program in order to parse the working plan at once
        var groupedByProgramRows = tableRows.GroupBy(row => row.EducationalProgram);
        foreach (var groupedByProgramRow in groupedByProgramRows)
        {
            var curriculumCode = groupedByProgramRow.Key
                    [1..groupedByProgramRow.Key.IndexOf(':')] // slicing № and title
                .Replace(',', '-');
            
            var curriculumPath = GetFullCurriculumCode(curriculumCode);
            var curriculum = new DocxCurriculum(curriculumPath);
            var curriculumTitle =
                curriculumPath[(curriculumPath.LastIndexOfAny(new char[]{'/', '\\'}) + 1)..curriculumPath.LastIndexOf('.')];
            var parserDisciplines = curriculum.Disciplines;
            foreach (var tableRow in groupedByProgramRow)
            {
                var disciplineCode = tableRow.PedagogicalTask[..tableRow.PedagogicalTask.IndexOf(' ')];
                var disciplineFromParser =
                    parserDisciplines.FirstOrDefault(discipline => discipline.Code == disciplineCode);
                if (disciplineFromParser == null)
                {
                    throw new DisciplineNotFoundException(
                        $"Discipline with code {disciplineCode} not found in {curriculumCode} working plan.");
                }

                var lecturer = tableRow.Lecturer;
                var discipline =
                    CreateDiscipline(discipline: disciplineFromParser, tableRow: tableRow, curriculumCode: curriculumTitle);
                if (lecturers.ContainsKey(lecturer))
                {
                    lecturers[lecturer].Disciplines.Add(discipline);
                }
                else
                {
                    lecturers.Add(lecturer, new Lecturer
                    {
                        Name = lecturer,
                        Disciplines = new List<Discipline>{discipline}
                    });
                }
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
            var groupedByCodeDisciplines = lecturer.Disciplines.GroupBy(discipline => discipline.Code);
            foreach (var groupedByCodeDiscipline in groupedByCodeDisciplines)
            {
                var groupedByTermDisciplines = groupedByCodeDiscipline.GroupBy(discipline => discipline.Term);
                foreach (var groupedByTermDiscipline in groupedByTermDisciplines)
                {
                    var contactLoad = 0;
                    Discipline exampleDiscipline;
                    var lectureDisciplines = groupedByTermDiscipline.Where(discipline =>
                        IsLecturerType(discipline.WorkType)).ToArray();
                    if (lectureDisciplines.Length != 0)
                    { 
                        exampleDiscipline = lectureDisciplines.First();
                        contactLoad = lectureDisciplines.Sum(lectureDiscipline => lectureDiscipline.ContactLoad);
                        mergedDisciplines.Add(new Discipline
                        {
                            Id = Guid.NewGuid(),
                            Code = exampleDiscipline.Code,
                            ContactLoad = contactLoad,
                            EducationalProgram = exampleDiscipline.EducationalProgram,
                            Term = exampleDiscipline.Term,
                            Content = $"{exampleDiscipline.Content} [{contactLoad}]",
                            WorkType = GetCommonWorkType(exampleDiscipline)
                        });

                    }

                    var nonLectureDisciplines = groupedByTermDiscipline
                        .Where(discipline => !IsLecturerType(discipline.WorkType)).ToArray();
                    if (nonLectureDisciplines.Length == 0) continue;
                    
                    var groupedByWorkTypeDisciplines = nonLectureDisciplines.GroupBy(disc => disc.WorkType).ToArray();
                    var firstGroupCount = groupedByWorkTypeDisciplines.First().Count();
                    
                    if (groupedByWorkTypeDisciplines.Any(groupedByWorkTypeDiscipline =>
                            groupedByWorkTypeDiscipline.Count() != firstGroupCount && 
                            groupedByWorkTypeDiscipline.Key.ToLower() != "лекции"))
                    {
                        throw new InvalidDisciplineWorkTypesCountException(
                            $"Discipline {groupedByCodeDiscipline.Key} in semester {groupedByTermDiscipline.Key} does not form equal groups.");
                    }
                    
                    contactLoad = groupedByWorkTypeDisciplines.Sum(group => group.First().ContactLoad);
                
                    exampleDiscipline = nonLectureDisciplines.First();
        
                    for (var i = 0; i < firstGroupCount; i++)
                    {
                        mergedDisciplines.Add(new Discipline
                        {
                            WorkType = GetCommonWorkType(exampleDiscipline),
                            Id = Guid.NewGuid(),
                            Code = exampleDiscipline.Code,
                            ContactLoad = contactLoad,
                            EducationalProgram = exampleDiscipline.EducationalProgram,
                            Term = exampleDiscipline.Term,
                            Content = $"{exampleDiscipline.Content} [{contactLoad}]"
                        });
                    } 
                }
            }

            updatedLecturers.Add((key, lecturer with { Disciplines = mergedDisciplines }));
        }

        updatedLecturers.ForEach(pair => lecturers[pair.Item1] = pair.Item2);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="curriculumCode">Code of curriculum</param>
    /// <exception cref="PLVisualizerApiNotFoundException">Throws when unable to find appropriate working plan.</exception>
    private static string GetFullCurriculumCode(string curriculumCode)
    {
        var pathTemplate = "Clients/DocxClient/WorkingPlans";
        var workingPlans = Directory.GetFiles(pathTemplate);
        var workingPlan = workingPlans.FirstOrDefault(plan => plan.Contains(curriculumCode));
        if (workingPlan == null)
        {
            throw new WorkingPlanNotFoundException(
                $"Could not find working plan appropriate to {curriculumCode} code.");
        }

        return workingPlan;
    }
    
    private static Discipline CreateDiscipline(
        CurriculumParser.Discipline discipline,
        ExcelTableRow tableRow,
        string curriculumCode)
    {
        var term = tableRow.Term;
        var implementation = discipline.Implementations.FirstOrDefault(implementation => implementation.Semester == term);
        if (implementation == null)
        {
            throw new DisciplineNotFoundException(
                $"Could not find {discipline.Code} {discipline.RussianName} in {term} semester.");
        }
        
        
        var workType = tableRow.WorkType;
        var workHours = GetImplementationWorkHours(implementation);
        
        var commonWorkType = string.Empty;
        const int practicesColumn = 3;
        const int lecturesColumn = 0;
        if (IsLecturerType(workType) && workHours[practicesColumn] != 0)
        {
            commonWorkType = "Лекции";
        }
        else if (!IsLecturerType(workType) && workHours[lecturesColumn] != 0)
        {
            commonWorkType = "Практики";
        }
        
        // specifies realization work education practices etc
        var realization = discipline.Implementations.First().Realization;
        var disciplineName = realization == null
            ? discipline.RussianName
            : $"{discipline.RussianName} ({realization})";
        
        var index = GetWorkTypeIndex(workType);
        // if appropriate work type not found or does not contain hours of contact load
        var contactLoad = index != -1 ? workHours[index] : 0;
        
        // if discipline has a credit and an exam we divide attestation by 2
        if (implementation.MonitoringTypes.Contains(' ') && workType.Contains("Промежуточная аттестация"))
        {
            contactLoad /= 2;
        }
        
        var content = commonWorkType == string.Empty
            ? $"{discipline.Code} {disciplineName} [{term}] [{curriculumCode}]"
            : $"{discipline.Code} {disciplineName} [{term}] [{commonWorkType}] [{curriculumCode}]";
                    
        return  new Discipline
        {
            Code = discipline.Code,
            EducationalProgram = curriculumCode,
            Term = term,
            Content = content,
            ContactLoad = contactLoad,
            WorkType = workType
        };
    }
    
    /// <summary>
    /// Gets array of implementation work hours corresponding to working plan work hours row.
    /// </summary>
    /// <param name="implementation">Discipline implementation.</param>
    private static int[] GetImplementationWorkHours(DisciplineImplementation implementation)
    {
        return implementation
            .WorkHours
            .Split()
            .Select(int.Parse)
            .ToArray();
    }
    
    /// <summary>
    /// Gets column corresponding to work type in working plan.
    /// </summary>
    /// <param name="workType">Work type.</param>
    private static int GetWorkTypeIndex(string workType)
    {
        // cases when some info provided in brackets
        if (workType.ToLower().Contains("промежуточная аттестация")) return 8;
        if (workType.ToLower().Contains("текущий контроль")) return 7;
        return workType.ToLower() switch
        {
            "лекции" => 0,
            "семинары" => 1,
            "консультации" => 2,
            "практические занятия" => 3,
            "лабораторные работы" => 4,
            "контрольные работы" => 5,
            "коллоквиумы" => 6,
            "в присутствии преподавателя" => 10,
            _ => -1
        };
    }

    private static bool IsLecturerType(string workType)
    {
        return (workType.ToLower() is "лекции" or "коллоквиумы" or "промежуточная аттестация (экз)" or "консультации");
    }

    /// <summary>
    /// Gets common work type of discipline.
    /// </summary>
    /// <param name="discipline">Discipline.</param>
    private static string GetCommonWorkType(Discipline discipline)
    {
        if (discipline.Content.Contains("[Практики]")) return "Практики";
        if (discipline.Content.Contains("[Лекции]")) return "Лекции";
        return string.Empty;
    }
}