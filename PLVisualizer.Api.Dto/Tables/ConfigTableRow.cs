namespace PlVisualizer.Api.Dto.Tables;

/// <summary>
/// Represents model of row in configuration table
/// </summary>
public class ConfigTableRow
{
    public string LecturerName { get; set; }
    public string Post { get; set; }
    public int InterestRate { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not ConfigTableRow configTableRow) return false;
        return Post == configTableRow.Post &&
               LecturerName == configTableRow.LecturerName &&
               InterestRate == configTableRow.InterestRate;
    }
}