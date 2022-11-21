using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.DocxClient;

public interface IDocxClient
{
    Dictionary<string,Lecturer> GetLecturersWithDisciplines(IEnumerable<XlsxTableRow> tableRows);
}