using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.DocxClient;

/// <summary>
/// Represents client that is using Curriculum Parser.
/// </summary>
public interface IDocxClient
{
    /// <summary>
    /// Gets lecturers dictionary via rows of input xlsx file.
    /// </summary>
    /// <param name="tableRows">Rows of xlsx file.</param>
    /// <returns>Dictionary of lecturers with disciplines merged into common types</returns>
    Dictionary<string,Lecturer> GetLecturersWithDisciplines(IEnumerable<ExcelTableRow> tableRows);
}