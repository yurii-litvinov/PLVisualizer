using CurriculumParser;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Spreadsheet;
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
            var curriculumName = GetCurriculumName(pathTemplate, eduProgramCode);
            var parser = new DocxCurriculum($"{pathTemplate}/{curriculumName}");
            var parserDisciplines = parser.Disciplines;
            var groupedByNameRows = groupedByProgramRow.GroupBy(row => row.DisciplineName);
            
            foreach (var groupedByNameRow in groupedByNameRows)
            {
                var disciplineCode = groupedByNameRow.Key[..groupedByNameRow.Key.IndexOf(' ')];
                var disciplineFromParser =
                    parserDisciplines.FirstOrDefault(discipline => discipline.Code == disciplineCode);

                var currentLecturer = groupedByNameRow.First().Lecturer;
                if (lecturers.ContainsKey(currentLecturer))
                {
                    lecturers[currentLecturer].Disciplines.Add(CreateDiscipline(
                        implementations: disciplineFromParser!.Implementations,
                        curriculumName: curriculumName, groupedByNameRow: groupedByNameRow));
                    lecturers[currentLecturer].DistributedLoad +=
                        lecturers[currentLecturer].Disciplines[^1].ContactLoad;
                }
                else
                {
                    var discipline = CreateDiscipline(
                        implementations: disciplineFromParser!.Implementations,
                        curriculumName: curriculumName, groupedByNameRow: groupedByNameRow);
                    // remaining properties will be filled via config spreadsheet
                    lecturers.Add(groupedByNameRow.First().Lecturer, new Lecturer
                    {
                        Name = groupedByNameRow.First().Lecturer,
                        DistributedLoad = discipline.ContactLoad,
                        Disciplines = new List<Discipline> {discipline}
                    });
                }
            }
        }

        return lecturers;
    }

    private static string GetCurriculumName(string pathTemplate, string curriculumCode)
    {
        var workingPlans = Directory.GetFiles(pathTemplate);
        return workingPlans.FirstOrDefault(plan => plan.Contains(curriculumCode)) ?? string.Empty;
    }

    private  static int GetTermNumber(string term)
    {
        return int.Parse(term.Replace("Семестр", string.Empty));
    }

    private static int GetContactLoad(string workHours)
    {
        var castedHours = workHours.Split().Select(int.Parse).ToArray();
        return castedHours.Take(9).Sum() + castedHours[10];
    }

    private static Discipline CreateDiscipline(List<DisciplineImplementation> implementations, 
        string curriculumName,  IGrouping<string,XlsxTableRow> groupedByNameRow)
    {
        var contactLoad = GetContactLoad(implementations.First().WorkHours ?? string.Empty);
        var content = $"{groupedByNameRow.First().DisciplineName} [{contactLoad}] [{curriculumName}]";
        return  new Discipline
        {
            EducationalProgram = curriculumName,
            Terms = string.Join(' ', groupedByNameRow.Select(row => GetTermNumber(row.Term))).Trim(),
            Content = content,
            ContactLoad = contactLoad
        };
    }
}