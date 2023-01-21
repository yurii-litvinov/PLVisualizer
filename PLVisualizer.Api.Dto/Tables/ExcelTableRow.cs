namespace PlVisualizer.Api.Dto.Tables;

/// <summary>
/// Represents model of row in xlsx file
/// </summary>
public record ExcelTableRow(string Term, string DisciplineCode, string DisciplineName, string WorkType, string Lecturer, int Hours, string Audience);
