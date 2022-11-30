using System.Diagnostics.Contracts;
using System.Security.AccessControl;

namespace PlVisualizer.Api.Dto.Tables;

public class ExcelTableRow
{
    /// <summary>
    /// Term
    /// </summary>
    public int Term { get; set; }

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
    /// Type of study
    /// </summary>
    public string WorkType { get; set; }

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

    public override bool Equals(object? obj)
    {
        if (obj is not ExcelTableRow xlsxTableRow) return false;
        return Term == xlsxTableRow.Term &&
               Subdivision == xlsxTableRow.Subdivision &&
               PedagogicalTask == xlsxTableRow.PedagogicalTask &&
               DisciplineName == xlsxTableRow.DisciplineName &&
               WorkType == xlsxTableRow.WorkType &&
               Lecturer == xlsxTableRow.Lecturer &&
               SAPSubdivision2 == xlsxTableRow.SAPSubdivision2 &&
               SAPSubdivision1 == xlsxTableRow.SAPSubdivision1;
    }
}