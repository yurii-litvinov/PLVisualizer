using CurriculumParser;
using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients;

public class DocxClient
{
    public PlVisualizer.Api.Dto.Tables.Discipline[] GetRequiredDisciplines(XlsxTableRow[] tableRows)
    {
        var groupedRows = tableRows.GroupBy(row => row.EducationalProgram);
        foreach (var groupedRow in groupedRows)
        {
            var eduProgramCode = groupedRow.Key
                .Split(':')[0][1..] // slicing №
                .Replace(',', '-');
            
            var workingPlans = Directory.GetFiles("WorkingPlans");
            var workingPlan = Directory.Enum
        }
    }
}