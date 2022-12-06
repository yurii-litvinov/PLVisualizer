using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.DocxClient;
using PLVisualizer.BusinessLogic.Clients.ExcelClient;
using PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;
using Discipline = PlVisualizer.Api.Dto.Tables.Discipline;

namespace PLVisualizerTest.TestDocxClient;

public class TestDocxClient
{
    private IDocxClient docxClient = new DocxClient();
    private ISpreadsheetProvider spreadsheetProvider = new SpreadsheetProvider();
    private IExcelClient excelClient;
    
    private static object[] getLecturersWithDisciplinesTestCases =
    {
        new Lecturer[]
        {
            new ()
            {
                Name = "Литвинов Юрий Викторович", Disciplines = new List<Discipline>
                {
                    new () { Code = "058505",  Term = 1, ContactLoad = 15, WorkType = string.Empty,
                        Content = "058505 Учебная практика (научно-исследовательская работа) [1] [ВМ.5665-2021] [15]", EducationalProgram = "ВМ.5665-2021", },
                    new () { Code = "002212",  Term = 1, ContactLoad = 64, WorkType = string.Empty,
                        Content = "002212 Программирование [1] [СВ.5162-2022] [64]", EducationalProgram = "СВ.5162-2022"},
                    new () { Code = "002212",  Term = 2 , ContactLoad = 32, WorkType = string.Empty,
                        Content = "002212 Программирование [2] [СВ.5162-2022] [32]", EducationalProgram = "СВ.5162-2022"},
                    new () { Code = "002212",  Term = 3, ContactLoad = 32, WorkType = string.Empty,
                        Content = "002212 Программирование [3] [СВ.5162-2021] [32]", EducationalProgram = "СВ.5162-2021"},
                    new () { Code = "002212",  Term = 3, ContactLoad = 32,WorkType = string.Empty,
                        Content = "002212 Программирование [3] [СВ.5162-2021] [32]", EducationalProgram = "СВ.5162-2021"},
                    new () { Code = "002212",  Term = 3, ContactLoad = 32,WorkType = string.Empty,
                        Content = "002212 Программирование [3] [СВ.5162-2021] [32]", EducationalProgram = "СВ.5162-2021"},
                    new () { Code = "002212",  Term = 3, ContactLoad = 32,WorkType = string.Empty,
                        Content = "002212 Программирование [3] [СВ.5162-2021] [32]", EducationalProgram = "СВ.5162-2021"},
                    new () { Code = "002211",  Term = 1, ContactLoad = 48,WorkType = string.Empty,
                    Content = "002211 Информатика [1] [СВ.5162-2022] [48]", EducationalProgram = "СВ.5162-2022" },
                    new () { Code = "002211",  Term = 2, ContactLoad = 46, WorkType = string.Empty,
                        Content = "002211 Информатика [2] [СВ.5162-2022] [46]", EducationalProgram = "СВ.5162-2022" },
                    new () { Code = "064792",  Term = 3, ContactLoad = 16, 
                    Content = "064792 Учебная практика 1 (научно-исследовательская работа) [3] [СВ.5162-2021] [16]",
                    EducationalProgram = "СВ.5162-2021",WorkType = string.Empty, }
                }
            },
            new ()
            {
                Name = "Кириленко Яков Александрович", Disciplines = new List<Discipline>
                {
                    new () { Code = "064851", Term = 4, ContactLoad = 14, WorkType = string.Empty,
                        Content = "064851 Производственная практика (преддипломная) [4] [ВМ.5665-2021] [14]", EducationalProgram = "ВМ.5665-2021" },
                    new () { Code = "064851", Term = 4, ContactLoad = 14, WorkType = string.Empty,
                        Content = "064851 Производственная практика (преддипломная) [4] [ВМ.5665-2021] [14]", EducationalProgram = "ВМ.5665-2021" },
                    new () { Code = "002187",  Term = 4, ContactLoad = 30, WorkType = string.Empty,
                        Content = "002187 Структуры и алгоритмы компьютерной обработки данных [4] [СВ.5162-2021] [30]", EducationalProgram = "СВ.5162-2021" }
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
        excelClient = new ExcelClient();
        var tableRows = excelClient.GetTableRows(spreadsheetDocument);
        var lecturers = docxClient.GetLecturersWithDisciplines(tableRows).Values.ToArray();
        Assert.AreEqual(expectedLecturers.Count, lecturers.Length);
        for (var i = 0; i < expectedLecturers.Count; i++)
        {
            Assert.AreEqual(expectedLecturers[i].Post, lecturers[i].Post);
            Assert.AreEqual(expectedLecturers[i].Name, lecturers[i].Name);
            Assert.AreEqual(expectedLecturers[i].Standard, lecturers[i].Standard);
            Assert.AreEqual(expectedLecturers[i].DistributedLoad, lecturers[i].DistributedLoad);
            Assert.AreEqual(expectedLecturers[i].InterestRate, lecturers[i].InterestRate);
            // checking disciplines
            Assert.That(expectedLecturers[i].Equals(lecturers[i]));
        }
    }
}