using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.DocxClient;
using PLVisualizer.BusinessLogic.Clients.GoogleClient;
using PLVisualizer.BusinessLogic.Clients.ExcelClient;
using PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;
using PLVisualizer.BusinessLogic.Services;

namespace PLVisualizerTest.TestTablesService;

public class TestTablesService
{
    private IExcelClient excelClient = new ExcelClient();
    private IDocxClient docxClient = new DocxClient();
    private ISpreadsheetProvider spreadsheetProvider = new SpreadsheetProvider();
    private static string largeFileExcelPath = "../../../TestDocxClient/LargeFileTest.xlsx";
    private static readonly string spreadsheetId = "13iWusc8H38jwL1Mhmd9ApSGyjsNQo0SudIGtJTyBDxE";
    private static string severalConfigLecturersSheetTitle = "SeveralConfigLecturers";

    private static object[] getLecturersViaConfigTestCases =
    {
        new object[] { severalConfigLecturersSheetTitle, largeFileExcelPath,
            new Lecturer[]
        {
            new ()
            {
                Name = "Литвинов Юрий Викторович", RequiredLoad = 500, Position = "доцент", FullTimePercent = 100, DistributedLoad = 349,
                Disciplines = new List<Discipline>
                {
                    new () { Code = "058505",  Term = 1, ContactLoad = 15, WorkType = string.Empty,
                        Content = "058505 Учебная практика (научно-исследовательская работа) [1] [ВМ.5665-2021] [15]", EducationalProgram = "ВМ.5665-2021", },
                    new () { Code = "002212",  Term = 1, ContactLoad = 64, WorkType = string.Empty,
                        Content = "002212 Программирование [1] [СВ.5162-2022] [64]", EducationalProgram = "СВ.5162-2022"},
                    new () { Code = "002212",  Term = 2 , ContactLoad = 32, WorkType = string.Empty,
                        Content = "002212 Программирование [2] [СВ.5162-2022] [32]", EducationalProgram = "СВ.5162-2022"},
                    new () { Code = "002212",  Term = 3, ContactLoad = 32, WorkType = string.Empty,
                        Content = "002212 Программирование [3] [СВ.5162-2021] [32]", EducationalProgram = "СВ.5162-2021"},
                    new () { Code = "002212",  Term = 3, ContactLoad = 32, WorkType = string.Empty,
                        Content = "002212 Программирование [3] [СВ.5162-2021] [32]", EducationalProgram = "СВ.5162-2021"},
                    new () { Code = "002212",  Term = 3, ContactLoad = 32, WorkType = string.Empty,
                        Content = "002212 Программирование [3] [СВ.5162-2021] [32]", EducationalProgram = "СВ.5162-2021"},
                    new () { Code = "002212",  Term = 3, ContactLoad = 32, WorkType = string.Empty,
                        Content = "002212 Программирование [3] [СВ.5162-2021] [32]", EducationalProgram = "СВ.5162-2021"},
                    new () { Code = "002211",  Term = 1, ContactLoad = 48, WorkType = string.Empty,
                    Content = "002211 Информатика [1] [СВ.5162-2022] [48]", EducationalProgram = "СВ.5162-2022" },
                    new () { Code = "002211",  Term = 2, ContactLoad = 46, WorkType = string.Empty,
                        Content = "002211 Информатика [2] [СВ.5162-2022] [46]", EducationalProgram = "СВ.5162-2022" },
                    new () { Code = "064792",  Term = 3, ContactLoad = 16, WorkType = string.Empty,
                    Content = "064792 Учебная практика 1 (научно-исследовательская работа) [3] [СВ.5162-2021] [16]",
                    EducationalProgram = "СВ.5162-2021" }
                }
            },
            new ()
            {
                Name = "Кириленко Яков Александрович", RequiredLoad = 650, DistributedLoad = 58, FullTimePercent = 50, Position = "старший преподаватель",
                Disciplines = new List<Discipline>
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
        }
    };

    [Test]
    [TestCaseSource(nameof(getLecturersViaConfigTestCases))]
    public async Task Test_GetLecturersViaConfig_ReturnsCorrectModels(string sheetTitle, string xlsxPath, 
        Lecturer[] expectedLecturers)
    {
        var spreadsheetDocument = spreadsheetProvider.GetSpreadsheetDocument(xlsxPath);
        var googleClient = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        var tablesService = new TablesService(docxClient, excelClient);
        var lecturers = await tablesService.GetLecturersViaConfigAsync(
            spreadsheetDocument: spreadsheetDocument,
            spreadsheetId: spreadsheetId,
            sheetTitle: sheetTitle);
        Assert.AreEqual(expectedLecturers.Length, lecturers.Length);
        for (var i = 0; i < expectedLecturers.Length; i++)
        {
            Assert.That(expectedLecturers[i].Equals(lecturers[i]));
        }
    }

    private static string severalLecturersSheetTitle = "SeveralLecturers";

    private static object[] getLecturersViaLecturersTableTestCases =
    {
        new object[]
        {
            severalLecturersSheetTitle, new Lecturer[]
            {
                new()
                {
                    Name = "Литвинов Юрий Викторович", Position = "доцент", FullTimePercent = 100, DistributedLoad = 241,
                    RequiredLoad = 500, Disciplines =
                        new List<Discipline>
                        {
                            new()
                            {
                                Code = "058505", ContactLoad = 9, 
                                Content =
                                    "058505 Учебная практика (научно-исследовательская работа) [3] [ВМ.5665-2021] [9]",
                                EducationalProgram = "ВМ.5665-2021", Term = 3, WorkType = string.Empty
                            },
                            new()
                            {
                                Code = "002212", ContactLoad = 64, WorkType = string.Empty, Term = 1,
                                Content = "002212 Программирование [1] [СВ.5162-2022] [64]",
                                EducationalProgram = "СВ.5162-2022"
                            },
                            new()
                            {
                                Code = "002212", ContactLoad = 32, WorkType = string.Empty, Term = 2,
                                Content = "002212 Программирование [2] [СВ.5162-2022] [32]",
                                EducationalProgram = "СВ.5162-2022"
                            },
                            new()
                            {
                                Code = "002212", ContactLoad = 32, WorkType = string.Empty, Term = 3,
                                Content = "002212 Программирование [3] [СВ.5162-2022] [32]",
                                EducationalProgram = "СВ.5162-2022"
                            },
                            new()
                            {
                                Code = "002211", ContactLoad = 36,
                                Content = "002211 Информатика [1] [СВ.5162-2022] [36]",
                                EducationalProgram = "СВ.5162-2022", WorkType = string.Empty, Term = 1
                            },
                            new()
                            {
                                Code = "002211", ContactLoad = 32,
                                Content = "002211 Информатика [2] [СВ.5162-2022] [32]",
                                EducationalProgram = "СВ.5162-2022", WorkType = string.Empty, Term = 2
                            },
                            new()
                            {
                                Code = "002211", ContactLoad = 36,
                                Content = "002211 Информатика [3] [СВ.5162-2022] [36]",
                                EducationalProgram = "СВ.5162-2022", WorkType = string.Empty, Term = 3
                            }
                        }
                },
                new()
                {
                    Name = "Кириленко Яков Александрович", Position = "старший преподаватель", FullTimePercent = 50,
                    DistributedLoad = 46, RequiredLoad = 650, Disciplines = new List<Discipline>()
                    {
                        new()
                        {
                            Code = "002187", ContactLoad = 32, Term = 4, WorkType = string.Empty,
                            EducationalProgram = "СВ.5162-2022",
                            Content =
                                "002187 Структуры и алгоритмы компьютерной обработки данных [4] [СВ.5162-2022] [32]",
                        },
                        new()
                        {
                            Code = "064851", ContactLoad = 14, Term = 1, WorkType = string.Empty,
                            EducationalProgram = "ВМ.5665-2021",
                            Content = "064851 Производственная практика (преддипломная) [1] [ВМ.5665-2021] [14]",
                        }
                    }
                }
            }
        }
    };
    
    [Test]
    [TestCaseSource(nameof(getLecturersViaLecturersTableTestCases))]
    public async Task Test_GetLecturersViaLecturersTable_ReturnsCorrectModels(string sheetTitle,
        Lecturer[] expectedLecturers)
    {
        var googleClient = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        var tablesService = new TablesService(docxClient, excelClient);
        var lecturers = await tablesService.GetLecturersViaLecturersTableAsync(spreadsheetId, sheetTitle);
        Assert.AreEqual(expectedLecturers.Length, lecturers.Length);
        for (var i = 0; i < expectedLecturers.Length; i++)
        {
            Assert.AreEqual(expectedLecturers[i].Name, lecturers[i].Name);
            Assert.AreEqual(expectedLecturers[i].Position, lecturers[i].Position);
            Assert.AreEqual(expectedLecturers[i].RequiredLoad, lecturers[i].RequiredLoad);
            Assert.AreEqual(expectedLecturers[i].DistributedLoad, lecturers[i].DistributedLoad);
            Assert.AreEqual(expectedLecturers[i].FullTimePercent, lecturers[i].FullTimePercent);
            Assert.AreEqual(expectedLecturers[i], lecturers[i]);
        }
    }
}