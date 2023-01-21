namespace PLVisualizer.BusinessLogic.Extensions;

using PlVisualizer.Api.Dto.Exceptions.SpreadsheetsExceptions;
using PlVisualizer.Api.Dto.Tables;

/// <summary>
/// Helper class that adds information from configuration table to lecturers collection.
/// </summary>
public static class LecturersConfiguration
{
    /// <summary>
    /// Adds interest rate and post to lecturers according to configuration table information.
    /// Lecturers not listed in configuration table are being removed.
    /// </summary>
    /// <param name="lecturers">Lecturers to configure.</param>
    /// <param name="configTableRows">Configuration table rows.</param>
    public static IEnumerable<Lecturer> WithConfigInformation(this Dictionary<string, Lecturer> lecturers,
        IEnumerable<ConfigTableRow> configTableRows)
    {
        var lecturersViaConfig = new List<Lecturer>();
        foreach (var configTableRow in configTableRows)
        {
            var lecturerName = configTableRow.LecturerName.Split(' ')[0];
            if (lecturers.ContainsKey(lecturerName))
            {
                lecturers[lecturerName] = lecturers[lecturerName] with 
                {
                    Name = configTableRow.LecturerName,
                    Position = configTableRow.Position, 
                    FullTimePercent = configTableRow.FullTimePercent 
                };

                lecturersViaConfig.Add(lecturers[lecturerName]);
            }
        }

        return lecturersViaConfig;
    }

    /// <summary>
    /// Adds standard to lecturers according to their post.
    /// </summary>
    /// <param name="lecturers">Lecturers to configure.</param>
    public static IEnumerable<Lecturer> WithStandards(this IEnumerable<Lecturer> lecturers)
        => lecturers.Select(l => l with { RequiredLoad = GetStandard(l) });

    /// <summary>
    /// Adds distributed load to lecturers according to disciplines assigned to him.
    /// </summary>
    /// <param name="lecturers"></param>
    /// <returns></returns>
    public static IEnumerable<Lecturer> WithDistributedLoad(this IEnumerable<Lecturer> lecturers)
        => lecturers.Select(l => l with { DistributedLoad = l.Disciplines.Select(discipline => discipline.Load).Sum() });
    
    private static int GetStandard(Lecturer lecturer)
    {
        var isPractician = lecturer.Position.Contains("практик");
        if (isPractician)
        {
            return lecturer.Position.ToLower() switch
            {
                "доцент" => 650,
                "старший преподаватель" => 700,
                "ассистент" => 750,
                _ => throw new UnknownLecturerPositionException()
            };
        }

        return lecturer.Position.ToLower() switch
        {
            "профессор" => 250,
            "доцент" => 500,
            "старший преподаватель" => 650,
            "ассистент" => 600,
            _ => throw new UnknownLecturerPositionException()
        };
    }
}