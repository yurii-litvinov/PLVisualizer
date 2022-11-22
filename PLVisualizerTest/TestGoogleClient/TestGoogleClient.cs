using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.GoogleClient;


namespace PLVisualizerTest.TestGoogleClient;

public class TestGoogleClient
{
    private IGoogleClient googleClient = new GoogleClient();
    private static string spreadsheetId = "13iWusc8H38jwL1Mhmd9ApSGyjsNQo0SudIGtJTyBDxE";
    private static string severalLecturersSheet = "SeveralLecturers";
    private static string singleLecturerSheet = "SingleLecturer";
    
    private static Lecturer[] sampleLecturers = {
        new()
        {
            Name = "Литвинов Юрий Викторович", Post = "доцент", InterestRate = 100, DistributedLoad = 241,
            Standard = 500, Disciplines =
                new List<Discipline>
                {
                    new()
                    { Code = "058505", ContactLoad = 9, Content = "058505 Учебная практика (научно-исследовательская работа) [3] [ВМ.5665-2021] [9]",
                        EducationalProgram = "ВМ.5665-2021", Term = 3, WorkType = string.Empty},
                    new()
                    {Code = "002212", ContactLoad = 64, WorkType = string.Empty, Term = 1,
                        Content = "002212 Программирование [1] [СВ.5162-2022] [64]", EducationalProgram = "СВ.5162-2022"},
                    new()
                    {Code = "002212", ContactLoad = 32, WorkType = string.Empty, Term = 2,
                        Content = "002212 Программирование [2] [СВ.5162-2022] [32]", EducationalProgram = "СВ.5162-2022"},
                    new()
                    {Code = "002212", ContactLoad = 32, WorkType = string.Empty, Term = 3,
                        Content = "002212 Программирование [3] [СВ.5162-2022] [32]", EducationalProgram = "СВ.5162-2022"},
                    new()
                    {Code = "002211", ContactLoad = 36, Content = "002211 Информатика [1] [СВ.5162-2022] [36]",
                        EducationalProgram = "СВ.5162-2022", WorkType = string.Empty, Term = 1 },
                    new()
                    {Code = "002211", ContactLoad = 32, Content = "002211 Информатика [2] [СВ.5162-2022] [32]",
                        EducationalProgram = "СВ.5162-2022", WorkType = string.Empty, Term = 2 },
                    new()
                    {Code = "002211", ContactLoad = 36, Content = "002211 Информатика [3] [СВ.5162-2022] [36]",
                        EducationalProgram = "СВ.5162-2022", WorkType = string.Empty, Term = 3 }
                }
        },
        new()
        {
            Name = "Кириленко Яков Александрович", Post = "старший преподаватель", InterestRate = 50,
            DistributedLoad = 46, Standard = 650, Disciplines = new List<Discipline>()
            {
                new() { Code = "002187", ContactLoad = 32, Term = 4, WorkType = string.Empty, EducationalProgram = "СВ.5162-2022",
                    Content = "002187 Структуры и алгоритмы компьютерной обработки данных [4] [СВ.5162-2022] [32]",},
                new()
                {Code = "064851", ContactLoad = 14, Term = 1, WorkType = string.Empty, EducationalProgram = "ВМ.5665-2021",
                    Content = "064851 Производственная практика (преддипломная) [1] [ВМ.5665-2021] [14]",}
            }
        }
    };

    private static object[] getLecturersTestCases =
    {
        new object[]
        {
            singleLecturerSheet, new[] {sampleLecturers[0]}
        },
        new object[]
        {
            severalLecturersSheet, sampleLecturers
        }
    };
    
    [Test]
    [TestCaseSource(nameof(getLecturersTestCases))]
    public async Task Test_GoogleClient_ReturnsCorrectLecturerModels(string sheetTitle, Lecturer[] expectedLecturers)
    {
        var lecturers = await googleClient.GetLecturersAsync(spreadsheetId, sheetTitle);
        Assert.AreEqual(expectedLecturers.Length, lecturers.Length);
        for (var i = 0; i < expectedLecturers.Length; i++)
        {
            Assert.That(expectedLecturers[i].Equals(lecturers[i]));
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
    public async Task Test_GoogleClient_ReturnsCorrectConfigTableRowModels(
        string sheetTitle, ConfigTableRow[] expectedTableRows)
    {
        var configTableRows = await googleClient.GetConfigTableRowsAsync(spreadsheetId, sheetTitle: sheetTitle);
        for (var i = 0; i < expectedTableRows.Length; i++)
        {
            Assert.That(expectedTableRows[i].Equals(configTableRows[i]));
        }
        
    }

    private static string exportLecturersSheet = "ExportLecturers";
    
    private static object[] exportLecturersTestCases = 
    {
        new object [] {sampleLecturers, exportLecturersSheet}
    };

    [Test]
    [TestCaseSource(nameof(exportLecturersTestCases))]
    public async Task Test_GoogleClient_ModelsFromGetLecturersToExportLecturersResultAreTheSame(Lecturer[] lecturers, string sheetTitle)
    {
        await googleClient.ExportLecturersAsync(spreadsheetId, lecturers, sheetTitle);
        var responseLecturers =  await googleClient.GetLecturersAsync(spreadsheetId, sheetTitle);
        Assert.AreEqual(lecturers.Length, responseLecturers.Length);
        for (var i = 0; i < lecturers.Length; i++)
        {
            Assert.That(lecturers[i].Equals(responseLecturers[i]));
        }
    }
}