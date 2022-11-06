using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NUnit.Framework;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.SpreadsheetsClient;
using PLVisualizer.BusinessLogic.Clients.XlsxClient;
using PLVisualizerTest.Extensions;

namespace PLVisualizerTest;

public class SpreadsheetsClientTest
{
    private SpreadsheetsClient spreadsheetsClient = new();
    private static string spreadsheetId = "13iWusc8H38jwL1Mhmd9ApSGyjsNQo0SudIGtJTyBDxE";
    private static string severalLecturersSheet = "SeveralLecturers";
    private static string singleLecturerSheet = "SingleLecturer";

    [SetUp]
    public void Setup()
    {

    }

    private static object[] getLecturersTestCases =
    {
        new object[]
        {
            severalLecturersSheet, new Lecturer[]
            {
                new()
                {
                    Name = "Литвинов Юрий Викторович", Post = "доцент", InterestRate = 100, DistributedLoad = 209, Standard = 500, Disciplines =
                        new List<Discipline>
                        {
                            new()
                            { Code = "058505", ContactLoad = 9, Content = "058505 Учебная практика (научно-исследовательская работа) [9] [BM.5665, осень]", EducationalProgram = "BM.5665"
                            },
                            new()
                            { Code = "002212", ContactLoad = 128, Content = "002212 Программирование [128] [CB.5162, осень-весна]", EducationalProgram = "CB.5162" },
                            new()
                            { Code = "002211", ContactLoad = 64, Content = "002211 Информатика [64] [CB.5162, осень-весна]", EducationalProgram = "CB.5162" }
                        }
                },
                new()
                {
                    Name = "Кириленко Яков Александрович", Post = "старший преподаватель", InterestRate = 100, DistributedLoad = 46, Standard = 500, Disciplines = new List<Discipline>()
                    {
                        new()
                        { Code = "002187", ContactLoad = 32, Content = "002187 Структуры и алгоритмы компьютерной обработки данных [32] [CB.5162, весна]", EducationalProgram = "CB.5162" },
                        new()
                        { Code = "064851", ContactLoad = 14, Content = "064851 Производственная практика (преддипломная) [14] [BM.5665, весна]", EducationalProgram = "BM.5665" }
                    }
                }
            }
        },
        new object[]
        {
             singleLecturerSheet, new Lecturer[]
            {
                new()
                {
                    Name = "Литвинов Юрий Викторович", Post = "доцент", InterestRate = 100, DistributedLoad = 137, Standard = 500,
                    Disciplines =
                        new List<Discipline>
                        {
                            new()
                            { Code = "058505", ContactLoad = 9, Content = "058505 Учебная практика (научно-исследовательская работа) [9] [BM.5665, осень]", EducationalProgram = "BM.5665" },
                            new()
                            { Code = "002212", ContactLoad = 128, Content = "002212 Программирование [128] [CB.5162, осень-весна]", EducationalProgram = "CB.5162" },
                        }
                }
            }
        }
    };




    [Test]
    [TestCaseSource(nameof(getLecturersTestCases))]
    public async Task Test_SpreadsheetsClient_ReturnsCorrectLecturerModels(string sheetTitle, Lecturer[] expectedLecturers)
    {
        spreadsheetsClient.SetSheetTitle(sheetTitle);
        var lecturers = await spreadsheetsClient.GetLecturersAsync(spreadsheetId);
        Assert.AreEqual(expectedLecturers.Length, lecturers.Length);
        for (var i = 0; i < expectedLecturers.Length; i++)
        {
            Assert.That(expectedLecturers[i].EqualsTo(lecturers[i]));
        }
    }

    private static string severalConfigLecturersSheet = "SeveralConfigLecturers";
    private static string singleConfigLecturersSheet = "SingleConfigLecturer";

    private static object[] getConfigTableRowsTestCases =
    {
        new object[]
        {
            severalConfigLecturersSheet, new ConfigTableRow[]
            {
                new() { LecturerName = "Литвинов Юрий Викторович", Post = "доцент", InterestRate = 100 },
                new() { LecturerName = "Кириленко Яков Александрович", Post = "старший преподаватель", InterestRate = 50 },
                new() { LecturerName = "Брыксин Тимофей Александрович", Post = "доцент", InterestRate = 0 }
            },
        },
        new object[]
        {
            singleConfigLecturersSheet, new ConfigTableRow[]
            { new() { LecturerName = "Литвинов Юрий Викторович", Post = "доцент", InterestRate = 100 } }
        }
    };
    
    
    [Test]
    [TestCaseSource(nameof(getConfigTableRowsTestCases))]
    public async Task Test_SpreadsheetsClient_ReturnsCorrectConfigTableRowModels(
        string sheetTitle, ConfigTableRow[] expectedTableRows)
    {
        spreadsheetsClient.SetSheetTitle(sheetTitle);
        var configTableRows = await spreadsheetsClient.GetConfigTableRowsAsync(spreadsheetId);
        for (var i = 0; i < expectedTableRows.Length; i++)
        {
            Assert.That(expectedTableRows[i].EqualsTo(configTableRows[i]));
        }
        
    }
}