using System.Security.AccessControl;

namespace PlVisualizer.Api.Dto;

public class Lecturer
{
    public string Name { get; set; }
    public string Post { get; set; }
    public int InterestRate { get; set; }
    public string[] Disciplines { get; set; }
    public string[] DistributedLoad { get; set; }
    public int Standard { get; set; }
}