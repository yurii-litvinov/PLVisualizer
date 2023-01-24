namespace PlVisualizer.Api.Dto.Tables;

/// <summary>
/// Represents one item of distributed pedagogical load (one row in an Excel table).
/// </summary>
public record LoadDetail(string LoadType, int Hours, string Audience);

/// <summary>
/// Represents discipline
/// </summary>
public record Discipline(
    Guid Id,
    int TotalLoad,
    string Term,
    string Code,
    string Name,
    string GeneralWorkType,
    string Audience,
    IEnumerable<LoadDetail> LoadDetails
);
