namespace PlVisualizer.Api.Dto;


public class TableRow
{
    public string Lecturer { get; set; }
    public string Post { get; set; }
    public int InterestRate { get; set; }
    public List<string> DisciplineIds { get; set; }
    public Dictionary<string, int> DistributedLoad { get; set; }
    public int Standard { get; set; }
}