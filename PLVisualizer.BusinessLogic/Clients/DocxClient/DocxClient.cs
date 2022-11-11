using CurriculumParser;
using PlVisualizer.Api.Dto.Tables;
using Discipline = PlVisualizer.Api.Dto.Tables.Discipline;

namespace PLVisualizer.BusinessLogic.Clients.DocxClient;

public class DocxClient : IDocxClient
{
    public void FillDisciplinesTerms(IEnumerable<Discipline> disciplines)
    {
        var pathTemplate = "../../../../../PLVisualizer/PLVisualizer.BusinessLogic/Clients/DocxClient/WorkingPlans";
        var groupedByProgramDisciplines = disciplines.GroupBy(discipline => discipline.EducationalProgram);
        foreach (var  groupedByProgramDiscipline in groupedByProgramDisciplines)
        {
            var parser = new DocxCurriculum($"{pathTemplate}/{groupedByProgramDiscipline.Key}.docx");
            foreach (var discipline in groupedByProgramDiscipline)
            {
                var disciplineFromParser = parser.Disciplines.FirstOrDefault(disc => disc.Code == discipline.Code);
                discipline.Terms = string.Join(' ', disciplineFromParser.Implementations.Select(implementation => implementation.Semester));
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
            var eduProgramCode = groupedByProgramRow.Key
                [1..groupedByProgramRow.Key.IndexOf(':')] // slicing № and title
                .Replace(',', '-');

            var pathTemplate = "../../../../../PLVisualizer.BusinessLogic/Clients/DocxClient/WorkingPlans";
            var curriculumCode = GetCurriculumCode(pathTemplate, eduProgramCode);
            var parser = new DocxCurriculum($"{pathTemplate}/{curriculumCode}");
            var parserDisciplines = parser.Disciplines;
            var groupedByDisciplineNameRows = groupedByProgramRow.GroupBy(row => row.DisciplineName);
            
            foreach (var groupedByNameRow in groupedByDisciplineNameRows)
            {
                var disciplineCode = groupedByNameRow.Key[..groupedByNameRow.Key.IndexOf(' ')];
                var disciplineFromParser =
                    parserDisciplines.FirstOrDefault(discipline => discipline.Code == disciplineCode);

                var currentLecturer = groupedByNameRow.First().Lecturer;
                var discipline = CreateDiscipline(discipline: disciplineFromParser, curriculumCode: curriculumCode);
                if (lecturers.ContainsKey(currentLecturer))
                {
                    lecturers[currentLecturer].Disciplines.Add(discipline);
                }
                else
                {
                    // remaining properties will be filled via config spreadsheet
                    lecturers.Add(groupedByNameRow.First().Lecturer, new Lecturer
                    {
                        Name = groupedByNameRow.First().Lecturer,
                        Disciplines = new List<Discipline> {discipline}
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
                                   castedHours[workInLecturerPresenceColumn]).Sum(); 
    }

    private static string GetTerms(CurriculumParser.Discipline discipline)
    {
        return string.Join(' ', discipline.Implementations.Select(implementation => implementation.Semester));
    }
    
    private static Discipline CreateDiscipline(CurriculumParser.Discipline discipline, 
        string curriculumCode)
    {
        var contactLoad = GetContactLoad(discipline);
        var terms = GetTerms(discipline);
        var content = $"{discipline.Code} {discipline.RussianName} [{contactLoad}] [{curriculumCode}]";
        return  new Discipline
        {
            Code = discipline.Code,
            EducationalProgram = curriculumCode,
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