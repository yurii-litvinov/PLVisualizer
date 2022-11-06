using NUnit.Framework;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.XlsxClient;
using PLVisualizerTest.Extensions;

namespace PLVisualizerTest;

public class XlsxClientTests
{
    private XlsxClient xlsxClient = new XlsxClient();
    [SetUp]
    public void Setup()
    {
        xlsxClient.SetFile("../../../XlsxClientTest/test.xlsx");
    }

    private static XlsxTableRow[] testTableRows =
    {
        new()
        {
            Term = "Семестр 1", Subdivision = "Математико-механический факультет",
            PedagogicalTask = "058505 Учебная практика (научно-исследовательская работа)",
            DisciplineName = "Учебная практика (научно-исследовательская работа)",
            WorkType = "В присутствии преподавателя", Lecturer = "Литвинов Ю. В., доцент",
            SAPSubdivision2 = "Кафедра системного программирования",
            SAPSubdivision1 = "Факультет математико-механический",
            EducationalProgram = "№5665,2022: Математическое обеспечение и администрирование информационных систем"
        },
        new()
        {
            Term = "Семестр 1", Subdivision = "Математико-механический факультет",
            PedagogicalTask = "002212 Программирование",
            DisciplineName = "Программирование",
            WorkType = "Промежуточная аттестация (зач)", Lecturer = "Литвинов Ю. В., доцент",
            SAPSubdivision2 = "Кафедра системного программирования",
            SAPSubdivision1 = "Факультет математико-механический",
            EducationalProgram = "№5162,2022: Технологии программирования"
        }
    };

    [Test]
    public void Test_XlsxClient_ReturnsCorrectModels()
    {
        var tableRows = xlsxClient.GetTableRows();
        for (var i = 0; i < tableRows.Length; i++)
        {
            Assert.That(tableRows[i].EqualsTo(testTableRows[i]));
        }
    }
}