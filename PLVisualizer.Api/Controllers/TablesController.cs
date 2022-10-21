using Microsoft.AspNetCore.Mvc;
using PlVisualizer.Api.Dto;
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
    public async Task<ActionResult<Lecturer[]>> GetLecturers([FromRoute]string spreadsheetId)
    {
        try
        {
            return await tablesService.GetLecturers(spreadsheetId);
        }
        catch (SpreadsheetNotFoundException)
        {
            return NotFound();
        }
        
    }

    [HttpGet]
    [Route("{spreadsheetId}/disciplines")]
    public async Task<ActionResult<Discipline[]>> GetRequiredDisciplines([FromRoute] string spreadsheetId)
    {
        try
        {
            return await tablesService.GetRequiredDisciplines(spreadsheetId);
        }
        catch (SpreadsheetNotFoundException e)
        {
            return NotFound();
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