using System.Security.AccessControl;

namespace PlVisualizer.Api.Dto.Tables;

/// <summary>
/// Represents lecturer
/// </summary>
public class Lecturer
{
    public string Name { get; set; } = string.Empty;
    public string Post { get; set; } = string.Empty;
    public int InterestRate { get; set; }
    public List<Discipline> Disciplines { get; set; } = new ();
    public int DistributedLoad { get; set; }
    public int Standard { get; set; }
    
    public override bool Equals(object obj)
    {
        if (obj is not Lecturer lecturer) return false;
        if (Disciplines.Count != lecturer.Disciplines.Count) return false;
        var t = Disciplines.Intersect(lecturer.Disciplines).Count();
        if (Disciplines.Any(discipline => !lecturer.Disciplines.Contains(discipline)))
        {
            return false;
        }


        return Name == lecturer.Name &&
               DistributedLoad == lecturer.DistributedLoad &&
               Standard == lecturer.Standard &&
               Post == lecturer.Post &&
               InterestRate == lecturer.InterestRate;
    }

    protected bool Equals(Lecturer other)
    {
        return Name == other.Name && Post == other.Post && InterestRate == other.InterestRate && 
               Disciplines.Equals(other.Disciplines) && DistributedLoad == other.DistributedLoad && Standard == other.Standard;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Post, InterestRate, Disciplines, DistributedLoad, Standard);
    }
}