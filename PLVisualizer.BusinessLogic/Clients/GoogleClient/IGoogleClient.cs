namespace PLVisualizer.BusinessLogic.Clients.GoogleClient;

using PlVisualizer.Api.Dto.Tables;

/// <summary>
/// Represents client that is using Google API to import and export lecturers.
/// </summary>
public interface IGoogleClient
{
    /// <summary>
    /// Exports lecturers to specified Google Spreadsheet.
    /// </summary>
    /// <param name="lecturers">Lecturers to export.</param>
    Task ExportLecturersAsync(Lecturer[] lecturers);
    
    /// <summary>
    /// Gets lecturers from a Google Spreadsheet previously exported by the application.
    /// </summary>
    Task<Lecturer[]> GetLecturersAsync();
    
    /// <summary>
    /// Gets configuration table row models.
    /// </summary>
    Task<ConfigTableRow[]> GetConfigTableRowsAsync();
}