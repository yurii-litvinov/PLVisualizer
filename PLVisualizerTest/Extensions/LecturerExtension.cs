using System.Linq;
using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizerTest.Extensions;

public static class LecturerExtension
{
    public static bool EqualsTo(this Lecturer current, Lecturer another)
    {
        
        var disciplinesAreEqual = current.Disciplines.Count == another.Disciplines.Count;
        if (current.Disciplines.Where((t, i) => !t.EqualsTo(another.Disciplines[i])).Any())
        {
            return false;
        }

        return current.Name == another.Name &&
               current.DistributedLoad == another.DistributedLoad &&
               current.Standard == another.Standard &&
               current.Post == another.Post &&
               current.InterestRate == another.InterestRate &&
               disciplinesAreEqual;
    }
    
}