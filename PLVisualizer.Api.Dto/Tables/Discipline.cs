namespace PlVisualizer.Api.Dto.Tables;

public class Discipline
{
    public string Content { get; set; }
    public int ContactLoad { get; set; }
    public string Terms { get; set; }
    public string Code { get; set; }
    public string EducationalProgram { get; set; }
    public bool HasPracticesHours { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Discipline discipline) return false;
        return Code == discipline.Code &&
               Content == discipline.Content &&
               ContactLoad == discipline.ContactLoad &&
               EducationalProgram == discipline.EducationalProgram &&
               Terms == discipline.Terms &&
               HasPracticesHours == discipline.HasPracticesHours;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Content, ContactLoad, Terms, Code, EducationalProgram, HasPracticesHours);
    }
}