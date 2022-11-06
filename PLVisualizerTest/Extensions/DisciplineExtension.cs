using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizerTest.Extensions;

public static class DisciplineExtension
{
    public static bool EqualsTo(this Discipline current, Discipline another)
    {
        // since google client cant get terms without docx client
        // both terms are null is ok, fill them in service via docx client
        var termsElementAreEqual =  current.Terms == another.Terms;

        return current.Code == another.Code &&
               current.Content == another.Content &&
               current.ContactLoad == another.ContactLoad &&
               current.EducationalProgram == another.EducationalProgram &&
               termsElementAreEqual;

    }
}