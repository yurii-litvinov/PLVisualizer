namespace PLVisualizerTest.TestGoogleClient;

using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.GoogleClient;

public class TestGoogleClient
{
    private static string spreadsheetId = "13iWusc8H38jwL1Mhmd9ApSGyjsNQo0SudIGtJTyBDxE";
    private static string severalLecturersSheet = "SeveralLecturers";
    private static string singleLecturerSheet = "SingleLecturer";
    
    private static Lecturer[] sampleLecturers = {
        new()
        {
            Name = "Литвинов Юрий Викторович", Position = "доцент", FullTimePercent = 100, DistributedLoad = 241,
            RequiredLoad = 500, Disciplines =
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
            Name = "Кириленко Яков Александрович", Position = "старший преподаватель", FullTimePercent = 50,
            DistributedLoad = 46, RequiredLoad = 650, Disciplines = new List<Discipline>()
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
        var client = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        var lecturers = await client.GetLecturersAsync();
        Assert.AreEqual(expectedLecturers.Length, lecturers.Length);
        for (var i = 0; i < expectedLecturers.Length; i++)
        {
            Assert.AreEqual(expectedLecturers[i],(lecturers[i]));
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
                new(LecturerName: "Литвинов Юрий Викторович", Position: "доцент", FullTimePercent: 100),
                new(LecturerName: "Кириленко Яков Александрович", Position: "старший преподаватель", FullTimePercent: 50),
                new(LecturerName: "Брыксин Тимофей Александрович", Position: "доцент", FullTimePercent: 0)
            },
        },
        new object[]
        {
            singleConfigLecturersSheet, new ConfigTableRow[]
            { new(LecturerName: "Литвинов Юрий Викторович", Position: "доцент", FullTimePercent: 100) }
        }
    };
    
    
    [Test]
    [TestCaseSource(nameof(getConfigTableRowsTestCases))]
    public async Task Test_GoogleClient_ReturnsCorrectConfigTableRowModels(
        string sheetTitle, ConfigTableRow[] expectedTableRows)
    {
        var client = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        var configTableRows = await client.GetConfigTableRowsAsync();
        for (var i = 0; i < expectedTableRows.Length; i++)
        {
            Assert.AreEqual(expectedTableRows[i], configTableRows[i]);
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
        var client = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        await client.ExportLecturersAsync(lecturers);
        var responseLecturers =  await client.GetLecturersAsync();
        Assert.AreEqual(lecturers.Length, responseLecturers.Length);
        for (var i = 0; i < lecturers.Length; i++)
        {
            Assert.That(lecturers[i].Equals(responseLecturers[i]));
        }
    }

    private static string lecturersWithSingleDisciplineSheet = "LecturersWithSingleDiscipline";

    private static object[] lecturersWithSingleDisciplineTestCases =
    {
        new object[] {
            lecturersWithSingleDisciplineSheet,
            new Lecturer[]
            {
            new ()
            {Name = "Литвинов Юрий Викторович", DistributedLoad = 36, Position = "доцент", RequiredLoad = 500, FullTimePercent = 100,
                Disciplines = new List<Discipline>()
                {
                    new()
                    {
                        Code = "002211", Term = 3, EducationalProgram = "СВ.5162-2022", ContactLoad = 36, WorkType = string.Empty,
                        Content = "002211 Информатика [3] [СВ.5162-2022] [36]"
                    }
                } }, new ()
            {Name = "Кириленко Яков Александрович", DistributedLoad = 32, Position = "старший преподаватель", RequiredLoad = 650, FullTimePercent = 50,
                Disciplines = new List<Discipline>()
                {
                    new()
                    {
                        Code = "002187", Term = 4, EducationalProgram = "СВ.5162-2022", ContactLoad = 32, WorkType = string.Empty,
                        Content = "002187 Структуры и алгоритмы компьютерной обработки данных [4] [СВ.5162-2022] [32]"
                    }
                } }
        }}
    };

    [Test]
    [TestCaseSource(nameof(lecturersWithSingleDisciplineTestCases))]
    public async Task Test_GoogleClient_ReturnCorrectLecturerModelsWithSingleDiscipline(string sheetTitle,
        Lecturer[] expectedLecturers)
    {
        var client = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        var lecturers = await client.GetLecturersAsync();
        Assert.AreEqual(expectedLecturers.Length, lecturers.Length);
        for (var i = 0; i < expectedLecturers.Length; i++)
        {
            Assert.AreEqual(expectedLecturers[i], lecturers[i]);
        }
    }
    
    private static string lecturersWithoutDisciplinesSheet = "LecturersWithoutDisciplines";

    private static object[] lecturersWithoutDisciplinesTestCases =
    {
        new object[] {
            lecturersWithoutDisciplinesSheet,
            new Lecturer[]
            {
                new ()
                {Name = "Литвинов Юрий Викторович", DistributedLoad = 0, Position = "доцент", RequiredLoad = 500, FullTimePercent = 100,
                    Disciplines = new List<Discipline>()
                   }, 
                new ()
                {Name = "Кириленко Яков Александрович", DistributedLoad = 0, Position = "старший преподаватель", RequiredLoad = 650, FullTimePercent = 50,
                    Disciplines = new List<Discipline>()
                        
                }
            }}
    };

    [Test]
    [TestCaseSource(nameof(lecturersWithoutDisciplinesTestCases))]
    public async Task Test_GoogleClient_ReturnsCorrectLecturerModelsWithoutDisciplines(string sheetTitle,
        Lecturer[] expectedLecturers)
    {
        var client = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        var lecturers = await client.GetLecturersAsync();
        Assert.AreEqual(expectedLecturers.Length, lecturers.Length);
        for (var i = 0; i < expectedLecturers.Length; i++)
        {
            Assert.AreEqual(expectedLecturers[i], lecturers[i]);
        }
    }
}
