using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.DocxClient;
using PLVisualizer.BusinessLogic.Clients.XlsxClient;
using PLVisualizerTest.Extensions;

namespace PLVisualizerTest.TestDocxClient;

public class TestDocxClient
{
    private IDocxClient docxClient = new DocxClient();

    private static object[] termsFillingCases =
    {
        new object[] {new Discipline[]
        {
            new() { Code = "002187", EducationalProgram = "СВ.5162-2021" },
            new() { Code = "002211", EducationalProgram = "СВ.5162-2021" },
            new() { Code = "002212", EducationalProgram = "СВ.5162-2021" },
            new() { Code = "058505", EducationalProgram = "ВМ.5665-2021" }
        }, 
            new [] {"4", "1 2 3", "1 2 3", "1"} },
        new object[] {new Discipline[]
        {
            new () { Code = "003574", EducationalProgram = "СВ.5080-2021"},
            new () { Code = "003565", EducationalProgram = "СВ.5080-2021"},
            new () { Code = "003574", EducationalProgram = "СВ.5080-2022"},
            new () { Code = "003565", EducationalProgram = "СВ.5080-2022"}
        },
            new [] { "1 2", "1 2", "1 2", "1 2"}
        }
    };
    
    [Test]
    [TestCaseSource(nameof(termsFillingCases))] 
    public void Test_DocxClient_FillsTermsCorrectly(Discipline[] disciplines, IList<string> expectedTerms)
    {
        docxClient.FillDisciplinesTerms(disciplines);
        for (var i = 0; i < disciplines.Length; i++)
        {
            Assert.AreEqual(expectedTerms[i], disciplines[i].Terms);
        }
    }

    private static object[] getLecturersWithDisciplinesTestCases =
    {
        new Lecturer[]
        {
            new ()
            {
                Name = "Литвинов Юрий Викторович", Disciplines = new List<Discipline>
                {
                    new () { Code = "058595",  Terms = "1", ContactLoad = 15,
                        Content = "058505 Учебная практика (научно-исследовательская работа) [15] [ВМ.5665-2022]", EducationalProgram = "ВМ.5665-2022", },
                    new () { Code = "002212",  Terms = "1 2 3", ContactLoad = 128,
                        Content = "002212 Программирование [128] [СВ.5162-2022]", EducationalProgram = "СВ.5162-2022" },
                    new () { Code = "002211",  Terms = "1 2 3", ContactLoad = 104,
                    Content = "002211 Информатика [104] [СВ.5162-2022]", EducationalProgram = "СВ.5162-2022" }
                }
            },
            new ()
            {
                Name = "Кириленко Яков Александрович", Disciplines = new List<Discipline>
                {
                    new () { Code = "064851",  Terms = "4", ContactLoad = 12,
                        Content = "064851 Производственная практика (преддипломная) [12] [ВМ.5665-2022]", EducationalProgram = "ВМ.5665-2022" },
                    new () { Code = "002187",  Terms = "4", ContactLoad = 32,
                        Content = "002187 Структуры и алгоритмы компьютерной обработки данных [32] [СВ.5162-2022]", EducationalProgram = "ВМ.5162-2022" }
                }
            }
        }
    };

    [Test]
    [TestCaseSource(nameof(getLecturersWithDisciplinesTestCases))]
    public void Test_DocxClient_ReturnsCorrectLecturersModel(IList<Lecturer> expectedLecturers)
    {
        var xlsxClient = new XlsxClient();
        var tableRows = xlsxClient.GetTableRows("../../../TestDocxClient/test.xlsx");
        var lecturers = docxClient.GetLecturersWithDisciplines(tableRows).Values.ToArray();
        Assert.AreEqual(expectedLecturers.Count, tableRows.Length);
        for (var i = 0; i < expectedLecturers.Count; i++)
        {
            Assert.That(expectedLecturers[i].EqualsTo(lecturers[i]));
        }
    }
    
}