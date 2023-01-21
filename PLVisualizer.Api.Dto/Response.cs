namespace PlVisualizer.Api.Dto;

/// <summary>
/// Represents response when an exception is thrown in application
/// </summary>
public record Response(string Content, Exception Exception);
