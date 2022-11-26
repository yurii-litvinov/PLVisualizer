using Microsoft.AspNetCore.Mvc;
using PlVisualizer.Api.Dto.Tables;
using PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;
using PLVisualizer.BusinessLogic.Services;

namespace PlVisualizer.Controllers;

[ApiController]
[Route("tables")]
public class TablesController : Controller
{
    private readonly ISpreadsheetProvider spreadsheetProvider;
    private readonly ITablesService tablesService;
    public TablesController(ITablesService tablesService, ISpreadsheetProvider spreadsheetProvider)
    {
        this.tablesService = tablesService;
        this.spreadsheetProvider = spreadsheetProvider;
    }

    [HttpGet]
    [Route("import/{spreadsheetId}")]
    public async Task<ActionResult<Lecturer[]>> GetLecturersViaLecturersTableAsync([FromRoute]string spreadsheetId)
    { 
        return await tablesService.GetLecturersViaLecturersTableAsync(spreadsheetId);
    }

    [HttpPost]
    [Route("import/config/{spreadsheetId}")]
    public async Task<ActionResult<Lecturer[]>> GetLecturerViaConfigAsync([FromRoute] string spreadsheetId, [FromForm]IFormFile file)
    {
        var spreadsheetDocument = spreadsheetProvider.GetSpreadsheetDocument(file);
        return await tablesService.GetLecturersViaConfigAsync(spreadsheetId, spreadsheetDocument);
    }

    [HttpPost]
    [Route("export/{spreadsheetId}")]
    public async Task<ActionResult> ExportLecturersAsync(string spreadsheetId, [FromBody]Lecturer[] lecturers)
    {
        await tablesService.ExportLecturersAsync(spreadsheetId, lecturers);
        return Ok();
    }
}