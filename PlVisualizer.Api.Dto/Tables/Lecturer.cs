using System.Security.AccessControl;

namespace PlVisualizer.Api.Dto.Tables;

public class Lecturer
{
    public string Name { get; set; }
    public string Post { get; set; }
    public int InterestRate { get; set; }
    public List<Discipline> Disciplines { get; set; }
    public int DistributedLoad { get; set; }
    public int Standard { get; set; }
}