using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using PlVisualizer.Api.Dto.Tables;
using PlVisualizer.Api.Dto.Exceptions;
using PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;
using PLVisualizer.BusinessLogic.Services;

namespace PlVisualizer.Controllers;

[ApiController]
[Route("tables")]
public class TablesController : Controller
{
    private ISpreadsheetProvider spreadsheetProvider;
    private ITablesService tablesService;
    public TablesController(ITablesService tablesService, ISpreadsheetProvider spreadsheetProvider)
    {
        this.tablesService = tablesService;
        this.spreadsheetProvider = spreadsheetProvider;
    }

    [HttpGet]
    [Route("import/{spreadsheetId}")]
    public async Task<ActionResult<Lecturer[]>> GetLecturersViaLecturersTableAsync([FromRoute]string spreadsheetId)
    {
        try
        {
            return await tablesService.GetLecturersViaLecturersTableAsync(spreadsheetId);
        }
        catch (SpreadsheetNotFoundException)
        {
            return NotFound();
        }
        
    }

    [HttpPost]
    [Route("import/config/{spreadsheetId}")]
    public async Task<ActionResult<Lecturer[]>> GetLecturerViaConfigAsync([FromRoute] string spreadsheetId, [FromForm] IFormFile file)
    {
        try
        {
            var spreadsheetDocument = spreadsheetProvider.GetSpreadsheetDocument(file);
            return await tablesService.GetLecturersViaConfigAsync(spreadsheetId, spreadsheetDocument);
        }
        catch (SpreadsheetNotFoundException e)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("export/{spreadsheetId}")]
    public async Task<ActionResult> ExportLecturersAsync(string spreadsheetId, [FromBody]Lecturer[] lecturers)
    {
        try
        {
            await tablesService.ExportLecturersAsync(spreadsheetId, lecturers);
            return Ok();
        }
        catch (SpreadsheetNotFoundException e)
        {
            return NotFound();
        }
    }
}