using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.SpreadsheetsClient;
using PLVisualizer.BusinessLogic.Clients.XlsxClient;
using PLVisualizerTest.Extensions;

namespace PLVisualizerTest.TestSpreadsheetsClient;

public class TestSpreadsheetsClient
{
    private ISpreadsheetsClient spreadsheetsClient = new SpreadsheetsClient();
    private static string spreadsheetId = "13iWusc8H38jwL1Mhmd9ApSGyjsNQo0SudIGtJTyBDxE";
    private static string severalLecturersSheet = "SeveralLecturers";
    private static string singleLecturerSheet = "SingleLecturer";
    
    private static Lecturer[] sampleLecturers = {
        new()
        {
            Name = "Литвинов Юрий Викторович", Post = "доцент", InterestRate = 100, DistributedLoad = 201,
            Standard = 500, Disciplines =
                new List<Discipline>
                {
                    new()
                    {
                        Code = "058505", ContactLoad = 9,
                        Content = "058505 Учебная практика (научно-исследовательская работа) [9] [BM.5665-2022, осень]",
                        EducationalProgram = "BM.5665-2022"
                    },
                    new()
                    {
                        Code = "002212", ContactLoad = 128,
                        Content = "002212 Программирование [128] [CB.5162-2022, осень-весна]", EducationalProgram = "CB.5162-2022"
                    },
                    new()
                    {
                        Code = "002211", ContactLoad = 64, Content = "002211 Информатика [64] [CB.5162-2022, осень-весна]",
                        EducationalProgram = "CB.5162-2022"
                    }
                }
        },
        new()
        {
            Name = "Кириленко Яков Александрович", Post = "старший преподаватель", InterestRate = 100,
            DistributedLoad = 46, Standard = 500, Disciplines = new List<Discipline>()
            {
                new()
                {
                    Code = "002187", ContactLoad = 32,
                    Content = "002187 Структуры и алгоритмы компьютерной обработки данных [32] [CB.5162-2022, весна]",
                    EducationalProgram = "CB.5162-2022"
                },
                new()
                {
                    Code = "064851", ContactLoad = 14,
                    Content = "064851 Производственная практика (преддипломная) [14] [BM.5665-2022, весна]",
                    EducationalProgram = "BM.5665-2022"
                }
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
    public async Task Test_SpreadsheetsClient_ReturnsCorrectLecturerModels(string sheetTitle, Lecturer[] expectedLecturers)
    {
        var lecturers = await spreadsheetsClient.GetLecturersAsync(spreadsheetId, sheetTitle);
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
        var configTableRows = await spreadsheetsClient.GetConfigTableRowsAsync(spreadsheetId, sheetTitle: sheetTitle);
        for (var i = 0; i < expectedTableRows.Length; i++)
        {
            Assert.That(expectedTableRows[i].EqualsTo(configTableRows[i]));
        }
        
    }

    private static string exportLecturersSheet = "ExportLecturers";
    
    private static object[] exportLecturersTestCases = 
    {
        new object [] {sampleLecturers, exportLecturersSheet}
    };

    [Test]
    [TestCaseSource(nameof(exportLecturersTestCases))]
    public async Task Test_SpreadsheetsClient_ModelsFromGetLecturersToExportLecturersResultAreTheSame(Lecturer[] lecturers, string sheetTitle)
    {
        await spreadsheetsClient.ExportLecturersAsync(spreadsheetId, lecturers, sheetTitle);
        var responseLecturers =  await spreadsheetsClient.GetLecturersAsync(spreadsheetId, sheetTitle);
        Assert.AreEqual(lecturers.Length, responseLecturers.Length);
        for (var i = 0; i < lecturers.Length; i++)
        {
            Assert.That(lecturers[i].EqualsTo(responseLecturers[i]));
        }
    }
}