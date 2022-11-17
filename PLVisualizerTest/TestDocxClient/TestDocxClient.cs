using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CurriculumParser;
using NUnit.Framework;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.DocxClient;
using PLVisualizer.BusinessLogic.Clients.XlsxClient;
using PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;
using Discipline = PlVisualizer.Api.Dto.Tables.Discipline;

namespace PLVisualizerTest.TestDocxClient;

public class TestDocxClient
{
    private IDocxClient docxClient = new DocxClient();
    private ISpreadsheetProvider spreadsheetProvider = new SpreadsheetProvider();

    private static object[] termsAndPracticesHoursFillingCases =
    {
        new object[] {new Discipline[]
        {
            new() { Code = "002187", EducationalProgram = "СВ.5162-2021"},
            new() { Code = "002211", EducationalProgram = "СВ.5162-2021"},
            new() { Code = "002212", EducationalProgram = "СВ.5162-2021"},
            new() { Code = "058505", EducationalProgram = "ВМ.5665-2021"}
        }, 
            new [] {"4", "1 2 3", "1 2 3", "1"},
        },
        new object[] {new Discipline[]
        {
            new () { Code = "003574", EducationalProgram = "СВ.5080-2021"},
            new () { Code = "003565", EducationalProgram = "СВ.5080-2021"},
            new () { Code = "003574", EducationalProgram = "СВ.5080-2022"}, 
            new () { Code = "003565", EducationalProgram = "СВ.5080-2022"}
        },
            new [] { "1 2", "1 2", "1 2", "1 2"},
        }
    };
    
    [Test]
    [TestCaseSource(nameof(termsAndPracticesHoursFillingCases))] 
    public void Test_DocxClient_FillsTermsCorrectly(Discipline[] disciplines, 
        IList<string> expectedTerms)
    {
        docxClient.FillDisciplinesTermsAndPracticesHours(disciplines);
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
                    new () { Code = "058505",  Terms = "1", ContactLoad = 17,
                        Content = "058505 Учебная практика (научно-исследовательская работа) [17] [ВМ.5665-2021]", EducationalProgram = "ВМ.5665-2021", },
                    new () { Code = "002212",  Terms = "1 2 3", ContactLoad = 128,
                        Content = "002212 Программирование [128] [СВ.5162-2022]", EducationalProgram = "СВ.5162-2022"},
                    new () { Code = "002212",  Terms = "1 2 3", ContactLoad = 128,
                    Content = "002212 Программирование [128] [СВ.5162-2021]", EducationalProgram = "СВ.5162-2021" },
                    new () { Code = "002211",  Terms = "1 2 3", ContactLoad = 106,
                    Content = "002211 Информатика [106] [СВ.5162-2022]", EducationalProgram = "СВ.5162-2022" },
                    new () { Code = "064792",  Terms = "3 4", ContactLoad = 20,
                    Content = "064792 Учебная практика 1 (научно-исследовательская работа) [20] [СВ.5162-2021]", EducationalProgram = "СВ.5162-2021" }
                }
            },
            new ()
            {
                Name = "Кириленко Яков Александрович", Disciplines = new List<Discipline>
                {
                    new () { Code = "064851",  Terms = "4", ContactLoad = 14,
                        Content = "064851 Производственная практика (преддипломная) [14] [ВМ.5665-2021]", EducationalProgram = "ВМ.5665-2021" },
                    new () { Code = "002187",  Terms = "4", ContactLoad = 32,
                        Content = "002187 Структуры и алгоритмы компьютерной обработки данных [32] [СВ.5162-2021]", EducationalProgram = "СВ.5162-2021" }
                }
            }
        }
    };

    [Test]
    [TestCaseSource(nameof(getLecturersWithDisciplinesTestCases))]
    public void Test_DocxClient_ReturnsCorrectLecturersModel(IList<Lecturer> expectedLecturers)
    {
        var spreadsheetDocument =
            spreadsheetProvider.GetSpreadsheetDocument("../../../TestDocxClient/LargeFileTest.xlsx");
        var xlsxClient = new XlsxClient();
        var tableRows = xlsxClient.GetTableRows(spreadsheetDocument);
        var lecturers = docxClient.GetLecturersWithDisciplines(tableRows).Values.ToArray();
        Assert.AreEqual(expectedLecturers.Count, lecturers.Length);
        for (var i = 0; i < expectedLecturers.Count; i++)
        {
            Assert.That(expectedLecturers[i].Equals(lecturers[i]));
        }
    }
}