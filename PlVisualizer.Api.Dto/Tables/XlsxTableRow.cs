using System.Security.AccessControl;

namespace PlVisualizer.Api.Dto.Tables;

public class XlsxTableRow
{
    /// <summary>
    /// Term
    /// </summary>
    public string Term { get; set; }
    
    /// <summary>
    /// Faculty etc
    /// </summary>
    public string Subdivision { get; set; }
    
    /// <summary>
    /// Pedagogical task
    /// </summary>
    public string PedagogicalTask { get; set; }
    
    /// <summary>
    /// Discipline name
    /// </summary>
    public string DisciplineName { get; set; }
    
    /// <summary>
    /// Teacher name
    /// </summary>
    public string Lecturer { get; set; }
    
    /// <summary>
    /// Department
    /// </summary>
    public string SAPSubdivision2 { get; set; }
    
    /// <summary>
    /// Faculty etc
    /// </summary>
    public string SAPSubdivision1 { get; set; }
    public string EducationalProgram { get; set; }
}