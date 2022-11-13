using System.Security.AccessControl;

namespace PlVisualizer.Api.Dto.Tables;

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
        if (Disciplines.Intersect(lecturer.Disciplines).Count() != Disciplines.Count)
        {
            return false;
        }


        return Name == lecturer.Name &&
               DistributedLoad == lecturer.DistributedLoad &&
               Standard == lecturer.Standard &&
               Post == lecturer.Post &&
               InterestRate == lecturer.InterestRate;
    }

}