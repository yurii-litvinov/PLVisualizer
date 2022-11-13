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
}