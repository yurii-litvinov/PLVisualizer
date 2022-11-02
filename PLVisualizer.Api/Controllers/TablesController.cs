using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using PlVisualizer.Api.Dto.Tables;
using PlVisualizer.Api.Dto.Exceptions;
using PLVisualizer.BusinessLogic.Services;

namespace PlVisualizer.Controllers;

[ApiController]
[Route("tables")]
public class TablesController : Controller
{
    private ITablesService tablesService;
    public TablesController(ITablesService tablesService)
    {
        this.tablesService = tablesService;
    }

    [HttpGet]
    [Route("import/{spreadsheetId}")]
    public async Task<ActionResult<Lecturer[]>> GetLecturersViaLecturersTable([FromRoute]string spreadsheetId)
    {
        try
        {
            return await tablesService.GetLecturersViaLecturersTable(spreadsheetId);
        }
        catch (SpreadsheetNotFoundException)
        {
            return NotFound();
        }
        
    }
    
    [HttpPost]
    [Route("import/file")]
    public async Task<ActionResult> UploadFile([FromBody] IFormFile file)
    {
        try
        {
            await tablesService.UploadFile(file);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("import/config/{spreadsheetId}")]
    public async Task<ActionResult<Lecturer[]>> GetLecturerViaConfig([FromRoute] string spreadsheetId)
    {
        try
        {
            return await tablesService.GetLecturersViaConfig(spreadsheetId);
        }
        catch (SpreadsheetNotFoundException e)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("export/{spreadsheetId}")]
    public async Task<ActionResult> UnloadLecturers(string spreadsheetId, [FromBody]Lecturer[] lecturers)
    {
        try
        {
            await tablesService.UnloadLecturers(spreadsheetId, lecturers);
            return Ok();
        }
        catch (SpreadsheetNotFoundException e)
        {
            return NotFound();
        }
    }
}