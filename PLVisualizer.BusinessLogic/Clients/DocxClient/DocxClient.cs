using CurriculumParser;
using PlVisualizer.Api.Dto.Exceptions.DocxExceptions;
using PlVisualizer.Api.Dto.Tables;
using Discipline = PlVisualizer.Api.Dto.Tables.Discipline;

namespace PLVisualizer.BusinessLogic.Clients.DocxClient;

public class DocxClient : IDocxClient
{
    public void FillDisciplinesTerms(IEnumerable<Discipline> disciplines)
    {
        var pathTemplate = "../../../../PLVisualizer.BusinessLogic/Clients/DocxClient/WorkingPlans";
        var groupedByProgramDisciplines = disciplines.GroupBy(discipline => discipline.EducationalProgram);
        foreach (var  groupedByProgramDiscipline in groupedByProgramDisciplines)
        {
            var parser = new DocxCurriculum($"{pathTemplate}/{groupedByProgramDiscipline.Key}.docx");
            foreach (var discipline in groupedByProgramDiscipline)
            {
                var disciplineFromParser = parser.Disciplines.FirstOrDefault(disc => disc.Code == discipline.Code);
                discipline.Terms = GetTerms(disciplineFromParser);
            }
        }
        
    }

    public Dictionary<string, Lecturer> GetLecturersWithDisciplines(IEnumerable<XlsxTableRow> tableRows)
    {
        var lecturers = new Dictionary<string, Lecturer>();
        
        //group by program in order to parse the working plan at once
        var groupedByProgramRows = tableRows.GroupBy(row => row.EducationalProgram);
        foreach (var groupedByProgramRow in groupedByProgramRows)
        {
            var curriculumCode = groupedByProgramRow.Key
                    [1..groupedByProgramRow.Key.IndexOf(':')] // slicing № and title
                .Replace(',', '-');

            var pathTemplate = "../../../../PLVisualizer.BusinessLogic/Clients/DocxClient/WorkingPlans";
            var curriculumPath = GetCurriculumCode(pathTemplate, curriculumCode);
            var curriculumTitle =
                curriculumPath[(curriculumPath.LastIndexOf('\\') + 1)..curriculumPath.LastIndexOf('.')];
            var parser = new DocxCurriculum(curriculumPath);
            var parserDisciplines = parser.Disciplines;
            var groupedByDisciplineNameRows = groupedByProgramRow.GroupBy(row => row.PedagogicalTask);

            foreach (var groupedByDisciplineName in groupedByDisciplineNameRows)
            {
                var disciplineCode = groupedByDisciplineName.Key[..groupedByDisciplineName.Key.IndexOf(' ')];
                var disciplineFromParser =
                    parserDisciplines.FirstOrDefault(discipline => discipline.Code == disciplineCode);
                if (disciplineFromParser == null)
                {
                    throw new DisciplineNotFoundException(
                        $"{disciplineCode} not found in {curriculumCode} working plan");
                }

                var lecturer = groupedByDisciplineName.First().Lecturer;
                var discipline =
                    CreateDiscipline(discipline: disciplineFromParser, curriculumTitle: curriculumTitle);
                if (lecturers.ContainsKey(lecturer))
                {
                    lecturers[lecturer].Disciplines.Add(discipline);
                }
                else
                {
                    // remaining properties will be filled via config spreadsheet
                    lecturers.Add(groupedByDisciplineName.First().Lecturer, new Lecturer
                    {
                        Name = groupedByDisciplineName.First().Lecturer,
                        Disciplines = new List<Discipline> { discipline }
                    });
                }
            }
        }
    

        return lecturers;
    }

    private static string GetCurriculumCode(string pathTemplate, string curriculumCode)
    {
        var workingPlans = Directory.GetFiles(pathTemplate);
        return workingPlans.FirstOrDefault(plan => plan.Contains(curriculumCode)) ?? string.Empty;
    }
    
    private static int GetContactLoad(CurriculumParser.Discipline discipline)
    {
        const int workInLecturerPresenceColumn = 10, classroomWorkColumnsCount = 9;
        return  discipline.Implementations.Select(implementation => 
            implementation.WorkHours.Split()
                .Select(int.Parse)
                .ToArray())
            .Select(castedHours => castedHours.Take(classroomWorkColumnsCount).Sum() + 
                                   castedHours[workInLecturerPresenceColumn])
            .Sum(); 
    }

    private static string GetTerms(CurriculumParser.Discipline discipline)
    {
        return string.Join(' ', discipline.Implementations.Select(implementation => implementation.Semester));
    }
    
    private static Discipline CreateDiscipline(CurriculumParser.Discipline discipline, 
        string curriculumTitle)
    {
        var realization = discipline.Implementations.First().Realization;
        var disciplineName = realization == null
            ? discipline.RussianName
            : $"{discipline.RussianName} ({realization})";
        var contactLoad = GetContactLoad(discipline);
        var terms = GetTerms(discipline);
        var content = $"{discipline.Code} {disciplineName} [{contactLoad}] [{curriculumTitle}]";
        return  new Discipline
        {
            Code = discipline.Code,
            EducationalProgram = curriculumTitle,
            Terms = terms,
            Content = content,
            ContactLoad = contactLoad,
            HasPracticesHours = DisciplineHasPracticesHours(discipline)
        };
    }

    private  static bool DisciplineHasPracticesHours(CurriculumParser.Discipline discipline)
    {
        const int practicesColumn = 3;
        return discipline.Implementations.Any(implementation =>
            implementation.WorkHours.Split().Select(int.Parse).ToArray()[practicesColumn] != 0);
    }
}