using System.Diagnostics;

namespace PlVisualizer.Api.Dto.Tables;

/// <summary>
/// Represents discipline
/// </summary>
public class Discipline
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public int ContactLoad { get; set; }
    public int Term { get; set; }
    public string Code { get; set; }
    public string EducationalProgram { get; set; }
    public string WorkType { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Discipline discipline) return false;
        return Code == discipline.Code &&
               Content == discipline.Content &&
               ContactLoad == discipline.ContactLoad &&
               EducationalProgram == discipline.EducationalProgram &&
               Term == discipline.Term &&
               WorkType == discipline.WorkType;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Content, ContactLoad, Term, WorkType, Code, EducationalProgram);
    }
}