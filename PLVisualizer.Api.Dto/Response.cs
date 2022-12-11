namespace PlVisualizer.Api.Dto;

/// <summary>
/// Represents response when an exception is thrown in application
/// </summary>
public class Response
{
    /// <summary>
    /// Status code
    /// </summary>
    public int StatusCode { get; set; }
    /// <summary>
    /// Exception message
    /// </summary>
    public string Content { get; set; }
}