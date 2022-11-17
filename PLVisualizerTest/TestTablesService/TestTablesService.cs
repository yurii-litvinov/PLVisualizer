using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.DocxClient;
using PLVisualizer.BusinessLogic.Clients.SpreadsheetsClient;
using PLVisualizer.BusinessLogic.Clients.XlsxClient;
using PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;
using PLVisualizer.BusinessLogic.Services;

namespace PLVisualizerTest.TestTablesService;

public class TestTablesService
{
    private IXlsxClient xlsxClient = new XlsxClient();
    private IDocxClient docxClient = new DocxClient();
    private ISpreadsheetsClient spreadsheetsClient = new SpreadsheetsClient();
    private ITablesService tablesService;
    private ISpreadsheetProvider spreadsheetProvider = new SpreadsheetProvider();
    private static string largeFileXlsxPath = "../../../TestDocxClient/LargeFileTest.xlsx";
    private static readonly string spreadsheetId = "13iWusc8H38jwL1Mhmd9ApSGyjsNQo0SudIGtJTyBDxE";
    private static string severalConfigLecturersSheetTitle = "SeveralConfigLecturers";

    [SetUp]
    public void Setup()
    {
        tablesService = new TablesService(docxClient, xlsxClient, spreadsheetsClient);
    }
    
    private static object[] getLecturersViaConfigTestCases =
    {
        new object[] { severalConfigLecturersSheetTitle, largeFileXlsxPath,
            new Lecturer[]
            {
                new ()
                {
                    Name = "Литвинов Юрий Викторович", Standard = 650, DistributedLoad = 399, InterestRate = 100,
                    Post = "доцент", Disciplines = new List<Discipline>
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
                    Name = "Кириленко Яков Александрович", Standard = 650,DistributedLoad = 46,InterestRate = 50, Post = "старший преподаватель",
                    Disciplines = new List<Discipline>
                    {
                        new () { Code = "064851",  Terms = "4", ContactLoad = 14,
                            Content = "064851 Производственная практика (преддипломная) [14] [ВМ.5665-2021]", EducationalProgram = "ВМ.5665-2021" },
                        new () { Code = "002187",  Terms = "4", ContactLoad = 32,
                            Content = "002187 Структуры и алгоритмы компьютерной обработки данных [32] [СВ.5162-2021]", EducationalProgram = "СВ.5162-2021" }
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
                    Name = "Литвинов Юрий Викторович", Post = "доцент", InterestRate = 100, DistributedLoad = 201,
                    Standard = 500, Disciplines =
                        new List<Discipline>
                        {
                            new()
                            {
                                Code = "058505", ContactLoad = 9, Terms = "1",
                                Content =
                                    "058505 Учебная практика (научно-исследовательская работа) [9] [ВМ.5665-2021, осень]",
                                EducationalProgram = "ВМ.5665-2021"
                            },
                            new()
                            {
                                Code = "002212", ContactLoad = 128, Terms = "1 2 3",
                                Content = "002212 Программирование [128] [СВ.5162-2022, осень-весна]",
                                EducationalProgram = "СВ.5162-2022"
                            },
                            new()
                            {
                                Code = "002211", ContactLoad = 64,Terms = "1 2 3",
                                Content = "002211 Информатика [64] [СВ.5162-2022, осень-весна]",
                                EducationalProgram = "СВ.5162-2022"
                            }
                        }
                },
                new()
                {
                    Name = "Кириленко Яков Александрович", Post = "старший преподаватель", InterestRate = 50,
                    DistributedLoad = 46, Standard = 650, Disciplines = new List<Discipline>()
                    {
                        new()
                        {
                            Code = "002187", ContactLoad = 32,
                            Content =
                                "002187 Структуры и алгоритмы компьютерной обработки данных [32] [СВ.5162-2022, весна]",
                            EducationalProgram = "СВ.5162-2022", Terms = "4"
                        },
                        new()
                        {
                            Code = "064851", ContactLoad = 14,Terms = "4",
                            Content = "064851 Производственная практика (преддипломная) [14] [ВМ.5665-2021, весна]",
                            EducationalProgram = "ВМ.5665-2021"
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
        var lecturers = await tablesService.GetLecturersViaLecturersTableAsync(spreadsheetId, sheetTitle);
        Assert.AreEqual(expectedLecturers.Length, lecturers.Length);
        for (var i = 0; i < expectedLecturers.Length; i++)
        {
            Assert.That(expectedLecturers[i].Equals(lecturers[i]));
        }
    }
}