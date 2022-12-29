using PlVisualizer.Api.Dto.Exceptions.SpreadsheetsExceptions;
using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Extensions;

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
            if (lecturers.ContainsKey(configTableRow.LecturerName))
            {
                lecturers[configTableRow.LecturerName].Post = configTableRow.Post;
                lecturers[configTableRow.LecturerName].InterestRate = configTableRow.InterestRate;
                lecturersViaConfig.Add(lecturers[configTableRow.LecturerName]);
            }
        }

        return lecturersViaConfig;
    }

    /// <summary>
    /// Adds standard to lecturers according to their post.
    /// </summary>
    /// <param name="lecturers">Lecturers to configure.</param>
    public static IEnumerable<Lecturer> WithStandards(this IEnumerable<Lecturer> lecturers)
    {
        var withStandards = lecturers as Lecturer[] ?? lecturers.ToArray();
        foreach (var lecturer in withStandards)
        {
            lecturer.Standard = GetStandard(lecturer);
        }

        return withStandards;
    }

    /// <summary>
    /// Adds distributed load to lecturers according to disciplines assigned to him.
    /// </summary>
    /// <param name="lecturers"></param>
    /// <returns></returns>
    public static IEnumerable<Lecturer> WithDistributedLoad(this IEnumerable<Lecturer> lecturers)
    {
        var withDistributedLoad = lecturers as Lecturer[] ?? lecturers.ToArray();
        foreach (var lecturer in withDistributedLoad)
        {
            lecturer.DistributedLoad = lecturer.Disciplines.Select(discipline => discipline.ContactLoad).Sum();
        }

        return withDistributedLoad;
    }

    private static int GetStandard(Lecturer lecturer)
    {
        var isPractician = lecturer.Post.Contains("практик");
        if (isPractician)
        {
            return lecturer.Post.ToLower() switch
            {
                "доцент" => 650,
                "старший преподаватель" => 700,
                "ассистент" => 750,
                _ => throw new UnknownLecturerPostException()
            };
        }
        return lecturer.Post.ToLower() switch
        {
            "профессор" => 250,
            "доцент" => 500,
            "старший преподаватель" => 650,
            "ассистент" => 600,
            _ => throw new UnknownLecturerPostException()
        };
    }
}