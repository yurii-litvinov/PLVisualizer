using CurriculumParser;
using PlVisualizer.Api.Dto.Exceptions.DocxExceptions;
using PlVisualizer.Api.Dto.Tables;
using Discipline = PlVisualizer.Api.Dto.Tables.Discipline;

namespace PLVisualizer.BusinessLogic.Clients.DocxClient;

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

            var pathTemplate = "../../../../PLVisualizer.BusinessLogic/Clients/DocxClient/WorkingPlans" ;
            var curriculumPath = GetFullCurriculumCode(pathTemplate, curriculumCode);
            var parser = new DocxCurriculum(curriculumPath);
            var curriculumTitle =
                curriculumPath[(curriculumPath.LastIndexOfAny(new char[]{'/', '\\'}) + 1)..curriculumPath.LastIndexOf('.')];
            var parserDisciplines = parser.Disciplines;
            foreach (var tableRow in groupedByProgramRow)
            {
                var disciplineCode = tableRow.PedagogicalTask[..tableRow.PedagogicalTask.IndexOf(' ')];
                var disciplineFromParser =
                    parserDisciplines.FirstOrDefault(discipline => discipline.Code == disciplineCode);
                if (disciplineFromParser == null)
                {
                    throw new DisciplineNotFoundException(
                        $"{disciplineCode} not found in {curriculumCode} working plan");
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

    private static void MergeLecturersDisciplines(Dictionary<string, Lecturer> lecturers)
    {
        foreach (var (_, lecturer) in lecturers)
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
                        throw new InvalidDisciplineWorkTypesCountException($"{groupedByCodeDiscipline.Key} discipline " +
                                                                           $"in {groupedByTermDiscipline.Key} term do not form equal groups");
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

            lecturer.Disciplines = mergedDisciplines;
        }
    }

    private static string GetFullCurriculumCode(string pathTemplate, string curriculumCode)
    {
        var workingPlans = Directory.GetFiles(pathTemplate);
        return workingPlans.FirstOrDefault(plan => plan.Contains(curriculumCode)) ?? string.Empty;
    }
    
    private static Discipline CreateDiscipline(CurriculumParser.Discipline discipline,
            ExcelTableRow tableRow,
            string curriculumCode)
    {
        var term = tableRow.Term;
        var implementation = discipline.Implementations.First(implementation => implementation.Semester == term);
        
        const int practicesColumn = 3;
        const int lecturesColumn = 0;
        
        var workType = tableRow.WorkType;
        var workHours = GetDisciplineWorkHours(implementation);
        var commonWorkType = string.Empty;
        if (IsLecturerType(workType) && workHours[practicesColumn] != 0)
        {
            commonWorkType = "Лекции";
        }
        else if (!IsLecturerType(workType) && workHours[lecturesColumn] != 0)
        {
            commonWorkType = "Практики";
        }
        
        var realization = discipline.Implementations.First().Realization;
        var disciplineName = realization == null
            ? discipline.RussianName
            : $"{discipline.RussianName} ({realization})";
        
        var index = GetIndexByWorkType(workType);
        var contactLoad = workHours[index];
        // if discipline has a credit and an exam we divine attestation by 2
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

    private static int[] GetDisciplineWorkHours(DisciplineImplementation implementation)
    {
        return implementation
            .WorkHours
            .Split()
            .Select(int.Parse)
            .ToArray();
    }

    private static int GetIndexByWorkType(string workType)
    {
        return workType.ToLower() switch
        {
            "лекции" => 0,
            "семинары" => 1,
            "консультации" => 2,
            "практические занятия" => 3,
            "лабораторные работы" => 4,
            "контрольные работы" => 5,
            "коллоквиумы" => 6,
            "текущий контроль" => 7,
            "промежуточная аттестация (экз)" => 8,
            "промежуточная аттестация (зач)" => 8,
            "в присутствии преподавателя" => 10,
            _ => -1
        };
    }

    private static bool IsLecturerType(string workType)
    {
        return (workType.ToLower() is "лекции" or "коллоквиумы" or "промежуточная аттестация (экз)" or "консультации");
    }

    private static string GetCommonWorkType(Discipline discipline)
    {
        if (discipline.Content.Contains("[Практики]")) return "Практики";
        if (discipline.Content.Contains("[Лекции]")) return "Лекции";
        return string.Empty;
    }
         
}