namespace PlVisualizer.Api.Dto.Tables;

/// <summary>
/// Represents lecturer
/// </summary>
public record struct Lecturer(string Name, string Position, int FullTimePercent, List<Discipline> Disciplines, int DistributedLoad, int RequiredLoad);
