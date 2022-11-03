using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.DocxClient;

public interface IDocxClient
{
    void FillDisciplinesTerms(Lecturer[] lecturers);
    Dictionary<string,Lecturer> GetLecturersWithDisciplines(XlsxTableRow[] tableRows);
}