using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.DocxClient;
using PLVisualizer.BusinessLogic.Clients.GoogleClient;
using PLVisualizer.BusinessLogic.Clients.ExcelClient;
using PLVisualizer.BusinessLogic.Extensions;
using PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;

namespace PLVisualizerTest.TestLecturersConfiguration;

public class TestLecturersConfiguration
{
    private ISpreadsheetProvider spreadsheetProvider = new SpreadsheetProvider();
    private IDocxClient docxClient = new DocxClient();
    private IExcelClient excelClient = new ExcelClient();
    private string spreadsheetId = "13iWusc8H38jwL1Mhmd9ApSGyjsNQo0SudIGtJTyBDxE";
    private static string largeExcelFilePath = "TestDocxClient/LargeFileTest.xlsx";

    private static string lecturerWithoutPracticesExcelPath =
        "TestLecturersConfiguration/SingleLecturerTest.xlsx";

    [SetUp]
    public void Setup()
    {
        docxClient = new DocxClient();
        excelClient = new ExcelClient();
    }

    private static string severalConfigLecturersSheet = "SeveralConfigLecturers";
    private static string singleConfigLecturersSheet = "SingleConfigLecturer";

    private static object[] lecturersConfigurationTestCases =
    {
        new object[]
        {
            singleConfigLecturersSheet, lecturerWithoutPracticesExcelPath,  new Lecturer[]
            {
                new () { Name = "Литвинов Юрий Викторович", Position = "доцент", FullTimePercent = 100, RequiredLoad = 500, DistributedLoad = 0},
            }
        },
        new object[]
        {
            singleConfigLecturersSheet, largeExcelFilePath, new Lecturer[] {
                new ()
            {
                Name = "Литвинов Юрий Викторович", Position = "доцент", FullTimePercent = 100, RequiredLoad = 500, DistributedLoad = 349
            }}
        },
        new object[]
        {
            severalConfigLecturersSheet, largeExcelFilePath,  new Lecturer[]
            {
                new () { Name = "Литвинов Юрий Викторович", Position = "доцент", FullTimePercent = 100, RequiredLoad = 500, DistributedLoad = 349},
                new () { Name = "Кириленко Яков Александрович", Position = "старший преподаватель", FullTimePercent = 50, RequiredLoad = 650, DistributedLoad = 58},
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
        var xlsxTableRows = excelClient.GetTableRows(spreadsheetDocument);
        var lecturers = docxClient.GetLecturersWithDisciplines(xlsxTableRows);
        var googleClient = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        var configTableRows = await googleClient.GetConfigTableRowsAsync();
        var filteredLecturers = lecturers.WithConfigInformation(configTableRows).ToArray();
        Assert.AreEqual(expectedLecturers.Length, filteredLecturers.Length);
        foreach (var expectedLecturer in expectedLecturers)
        {
            var lecturer =
                filteredLecturers.FirstOrDefault(filteredLecturer => filteredLecturer.Name == expectedLecturer.Name);
            Assert.AreEqual(expectedLecturer.Position, lecturer.Position);
            Assert.AreEqual(expectedLecturer.FullTimePercent, lecturer.FullTimePercent);
        }
    }

    [Test]
    [TestCaseSource(nameof(lecturersConfigurationTestCases))] 
    public async Task Test_WithStandards_UpgradesLecturersCorrectly(string sheetTitle,
        string xlsxPath, 
        Lecturer[] expectedLecturers)
    {
        var spreadsheetDocument = spreadsheetProvider.GetSpreadsheetDocument(xlsxPath);
        var xlsxTableRows = excelClient.GetTableRows(spreadsheetDocument);
        var lecturers = docxClient.GetLecturersWithDisciplines(xlsxTableRows);
        var googleClient = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        var configTableRows = await googleClient.GetConfigTableRowsAsync();
        var lecturersWithStandards =
            lecturers.WithConfigInformation(configTableRows)
            .WithStandards()
            .ToArray();
        Assert.AreEqual(expectedLecturers.Length, lecturersWithStandards.Length);
        foreach (var expectedLecturer in expectedLecturers)
        {
            var lecturer = lecturersWithStandards.FirstOrDefault(lecturerWithStandard =>
                lecturerWithStandard.Name == expectedLecturer.Name);
            Assert.AreEqual(expectedLecturer.RequiredLoad, lecturer.RequiredLoad);
        }
    }

    [Test]
    [TestCaseSource(nameof(lecturersConfigurationTestCases))]
    public async Task Test_WithDistributedLoad_UpgradesLecturersCorrectly(string sheetTitle,
        string xlsxPath,
        Lecturer[] expectedLecturers)
    {
        var spreadsheetDocument = spreadsheetProvider.GetSpreadsheetDocument(xlsxPath);
        var xlsxTableRows = excelClient.GetTableRows(spreadsheetDocument);
        var lecturers = docxClient.GetLecturersWithDisciplines(xlsxTableRows);
        var googleClient = await GoogleClient.Connect(spreadsheetId, sheetTitle);
        var configTableRows = await googleClient.GetConfigTableRowsAsync();
        var lecturersWithDistributedLoad =
            lecturers.WithConfigInformation(configTableRows)
                .WithDistributedLoad()
                .ToArray();
        Assert.AreEqual(expectedLecturers.Length, lecturersWithDistributedLoad.Length);
        foreach (var expectedLecturer in expectedLecturers)
        {
            var lecturer = lecturersWithDistributedLoad.FirstOrDefault(lecturerWithDistributedLoad =>
                lecturerWithDistributedLoad.Name == expectedLecturer.Name);
            Assert.AreEqual(expectedLecturer.DistributedLoad, lecturer.DistributedLoad);
        }
    }
}
