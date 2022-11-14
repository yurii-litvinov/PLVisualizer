using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.DocxClient;

public interface IDocxClient
{
    void FillDisciplinesTermsAndPracticesHours(IEnumerable<Discipline> disciplines);
    Dictionary<string,Lecturer> GetLecturersWithDisciplines(IEnumerable<XlsxTableRow> tableRows);
}