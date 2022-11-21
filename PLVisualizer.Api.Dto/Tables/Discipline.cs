namespace PlVisualizer.Api.Dto.Tables;

public class Discipline
{
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
               Term == discipline.Term;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Content, ContactLoad, Term, Code, EducationalProgram);
    }
}