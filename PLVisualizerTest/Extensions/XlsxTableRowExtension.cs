using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizerTest.Extensions;

public static class XlsxTableRowExtension
{
    public static bool EqualsTo(this XlsxTableRow current, XlsxTableRow anotherRow)
    {
        return current.Term == anotherRow.Term &&
               current.Subdivision == anotherRow.Subdivision &&
               current.PedagogicalTask == anotherRow.PedagogicalTask &&
               current.DisciplineName == anotherRow.DisciplineName &&
               current.WorkType == anotherRow.WorkType &&
               current.Lecturer == anotherRow.Lecturer &&
               current.SAPSubdivision2 == anotherRow.SAPSubdivision2 &&
               current.SAPSubdivision1 == anotherRow.SAPSubdivision1;
    }
    
}