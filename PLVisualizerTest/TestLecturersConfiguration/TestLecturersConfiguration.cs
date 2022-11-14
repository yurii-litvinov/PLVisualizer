using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NUnit.Framework;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.DocxClient;
using PLVisualizer.BusinessLogic.Clients.SpreadsheetsClient;
using PLVisualizer.BusinessLogic.Clients.XlsxClient;
using PLVisualizer.BusinessLogic.Extensions;
using PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;

namespace PLVisualizerTest.TestLecturersConfiguration;

public class TestLecturersConfiguration
{
    private ISpreadsheetProvider spreadsheetProvider = new SpreadsheetProvider();
    private IDocxClient docxClient = new DocxClient();
    private ISpreadsheetsClient spreadsheetsClient = new SpreadsheetsClient();
    private IXlsxClient xlsxClient = new XlsxClient();
    private string spreadsheetId = "13iWusc8H38jwL1Mhmd9ApSGyjsNQo0SudIGtJTyBDxE";
    private static string largeXlsxFilePath = "../../../TestDocxClient/LargeFileTest.xlsx";

    private static string lecturerWithoutPracticesXlsxPath =
        "../../../TestLecturersConfiguration/LecturerWithoutPracticesTest.xlsx";

    [SetUp]
    public void Setup()
    {
        docxClient = new DocxClient();
        spreadsheetsClient = new SpreadsheetsClient();
        xlsxClient = new XlsxClient();
    }

    private static string severalConfigLecturersSheet = "SeveralConfigLecturers";
    private static string singleConfigLecturersSheet = "SingleConfigLecturer";

    private static object[] lecturersConfigurationTestCases =
    {
        new object[]
        {
            singleConfigLecturersSheet, lecturerWithoutPracticesXlsxPath,  new Lecturer[]
            {
                new () { Name = "Литвинов Юрий Викторович", Post = "доцент", InterestRate = 100, Standard = 500, DistributedLoad = 17},
            }
        },
        new object[]
        {
            singleConfigLecturersSheet, largeXlsxFilePath, new Lecturer[] {
                new ()
            {
                Name = "Литвинов Юрий Викторович", Post = "доцент", InterestRate = 100, Standard = 650, DistributedLoad = 399
            }}
        },
        new object[]
        {
            severalConfigLecturersSheet, largeXlsxFilePath,  new Lecturer[]
            {
                new () { Name = "Литвинов Юрий Викторович", Post = "доцент", InterestRate = 100, Standard = 650, DistributedLoad = 399},
                new () { Name = "Кириленко Яков Александрович", Post = "старший преподаватель", InterestRate = 50, Standard = 700, DistributedLoad = 46},
            }
        }
    };
    
    [Test]
    [TestCaseSource(nameof(lecturersConfigurationTestCases))]
    public async Task Test_WithConfigInformation_FiltersAndUpgradesLecturersCorrectly(string sheetTitle,
        string xlsxPath,
        Lecturer[] expectedLecturers)
    {
        var spreadsheetDocument = spreadsheetProvider.GetSpreadsheetDocument(xlsxPath);
        var xlsxTableRows = xlsxClient.GetTableRows(spreadsheetDocument);
        var lecturers = docxClient.GetLecturersWithDisciplines(xlsxTableRows);
        var configTableRows = await spreadsheetsClient.GetConfigTableRowsAsync(spreadsheetId, sheetTitle);
        var filteredLecturers = lecturers.WithConfigInformation(configTableRows).ToArray();
        Assert.AreEqual(expectedLecturers.Length, filteredLecturers.Length);
        foreach (var expectedLecturer in expectedLecturers)
        {
            var lecturer =
                filteredLecturers.FirstOrDefault(filteredLecturer => filteredLecturer.Name == expectedLecturer.Name);
            Assert.True(lecturer != null);
            Assert.AreEqual(expectedLecturer.Post, lecturer?.Post);
            Assert.AreEqual(expectedLecturer.InterestRate, lecturer?.InterestRate);
        }
    }

    [Test]
    [TestCaseSource(nameof(lecturersConfigurationTestCases))] 
    public async Task Test_WithStandards_UpgradesLecturersCorrectly(string sheetTitle,
        string xlsxPath, 
        Lecturer[] expectedLecturers)
    {
        var spreadsheetDocument = spreadsheetProvider.GetSpreadsheetDocument(xlsxPath);
        var xlsxTableRows = xlsxClient.GetTableRows(spreadsheetDocument);
        var lecturers = docxClient.GetLecturersWithDisciplines(xlsxTableRows);
        var configTableRows = await spreadsheetsClient.GetConfigTableRowsAsync(spreadsheetId, sheetTitle);
        var lecturersWithStandards =
            lecturers.WithConfigInformation(configTableRows)
            .WithStandards()
            .ToArray();
        Assert.AreEqual(expectedLecturers.Length, lecturersWithStandards.Length);
        foreach (var expectedLecturer in expectedLecturers)
        {
            var lecturer = lecturersWithStandards.FirstOrDefault(lecturerWithStandard =>
                lecturerWithStandard.Name == expectedLecturer.Name);
            Assert.AreEqual(expectedLecturer.Standard, lecturer?.Standard);
        }
    }

    [Test]
    [TestCaseSource(nameof(lecturersConfigurationTestCases))]
    public async Task Test_WithDistributedLoad_UpgradesLecturersCorrectly(string sheetTitle,
        string xlsxPath,
        Lecturer[] expectedLecturers)
    {
        var spreadsheetDocument = spreadsheetProvider.GetSpreadsheetDocument(xlsxPath);
        var xlsxTableRows = xlsxClient.GetTableRows(spreadsheetDocument);
        var lecturers = docxClient.GetLecturersWithDisciplines(xlsxTableRows);
        var configTableRows = await spreadsheetsClient.GetConfigTableRowsAsync(spreadsheetId, sheetTitle);
        var lecturersWithDistributedLoad =
            lecturers.WithConfigInformation(configTableRows)
                .WithDistributedLoad()
                .ToArray();
        Assert.AreEqual(expectedLecturers.Length, lecturersWithDistributedLoad.Length);
        foreach (var expectedLecturer in expectedLecturers)
        {
            var lecturer = lecturersWithDistributedLoad.FirstOrDefault(lecturerWithDistributedLoad =>
                lecturerWithDistributedLoad.Name == expectedLecturer.Name);
            Assert.AreEqual(expectedLecturer.DistributedLoad, lecturer?.DistributedLoad);
        }
    }
    
    
}