namespace PlVisualizer.Api.Dto;

/// <summary>
/// Represents response when an exception is thrown in application
/// </summary>
public class Response
{
    /// <summary>
    /// Exception message
    /// </summary>
    public string Content { get; set; }
    
    /// <summary>
    /// Exception
    /// </summary>
    public Exception Exception { get; set; }
}