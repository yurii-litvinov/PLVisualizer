using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Clients.DocxClient;

namespace PLVisualizerTest.TestDocxClient;

public class TestDocxClient
{
    private IDocxClient docxClient = new DocxClient();

    private static object[] termsFillingCases =
    {
        new object[] {new Discipline[]
        {
            new() { Code = "002187", EducationalProgram = "СВ.5162-2021" },
            new() { Code = "002211", EducationalProgram = "СВ.5162-2021" },
            new() { Code = "002212", EducationalProgram = "СВ.5162-2021" },
            new() { Code = "058505", EducationalProgram = "ВМ.5665-2021" }
        }, new [] {"4", "1 2 3", "1 2 3", "1"} },
        new object[] {new Discipline[]
        {
            new () { Code = "003574", EducationalProgram = "СВ.5080-2021"},
            new () { Code = "003565", EducationalProgram = "СВ.5080-2021"},
            new () { Code = "003574", EducationalProgram = "СВ.5080-2022"},
            new () { Code = "003565", EducationalProgram = "СВ.5080-2022"}
        }, new [] { "1 2", "1 2", "1 2", "1 2"}
        }
    };
    
    [Test]
    [TestCaseSource(nameof(termsFillingCases))] 
    public void Test_DocxClient_FillsTermsCorrectly(Discipline[] disciplines, IList<string> expectedTerms)
    {
        docxClient.FillDisciplinesTerms(disciplines);
        for (var i = 0; i < disciplines.Length; i++)
        {
            Assert.AreEqual(expectedTerms[i], disciplines[i].Terms);
        }
    }
}