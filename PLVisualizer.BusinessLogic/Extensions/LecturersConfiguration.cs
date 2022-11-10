using PlVisualizer.Api.Dto.Exceptions;
using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Extensions;

public static class LecturersConfiguration
{
    public static Lecturer[] WithConfigInformation(this Dictionary<string, Lecturer> lecturers,
        ConfigTableRow[] configTableRows)
    {
        foreach (var configTableRow in configTableRows)
        {
            if (lecturers.ContainsKey(configTableRow.LecturerName))
            {
                lecturers[configTableRow.LecturerName].Post = configTableRow.Post;
                lecturers[configTableRow.LecturerName].InterestRate = configTableRow.InterestRate;
            }
        }

        return lecturers.Values.ToArray();
    }

    public static Lecturer[] WithStandards(this Lecturer[] lecturers)
    {
        foreach (var lecturer in lecturers)
        {
            lecturer.Standard = GetStandard(lecturer);
        }

        return lecturers;
    }

    public static Lecturer[] WithDistributedLoad(this Lecturer[] lecturers)
    {
        foreach (var lecturer in lecturers)
        {
            lecturer.DistributedLoad = lecturer.Disciplines.Select(discipline => discipline.ContactLoad).Sum();
        }

        return lecturers;
    }

    private static int GetStandard(Lecturer lecturer)
    {
        var isPractician = lecturer.Disciplines.Any(discipline => discipline.HasPracticesHours);
        if (isPractician)
        {
            return lecturer.Post.ToLower() switch
            {
                "профессор" => 250,
                "доцент" => 500,
                "старший преподаватель" => 650,
                "ассистент" => 600,
                _ => throw new UnknownLecturerPostException()
            };
        }
        return lecturer.Post.ToLower() switch
        {
            "доцент" => 650,
            "старший преподаватель" => 700,
            "ассистент" => 750,
            _ => throw new UnknownLecturerPostException()
        };
    }
}