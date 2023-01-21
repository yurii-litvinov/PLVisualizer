namespace PlVisualizer.Api.Dto.Tables;

/// <summary>
/// Represents discipline
/// </summary>
public record Discipline(Guid Id, string Content, int Load, string Term, string Code, string Name, string WorkType, string Audience);
