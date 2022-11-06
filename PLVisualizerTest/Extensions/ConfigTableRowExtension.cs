using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizerTest.Extensions;

public static class ConfigTableRowExtension
{
    public static bool EqualsTo(this ConfigTableRow current, ConfigTableRow another)
    {
        return current.Post == another.Post &&
               current.LecturerName == another.LecturerName &&
               current.InterestRate == another.InterestRate;
    }
}