namespace HostManagementAPI;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HostController : ControllerBase
{
    private readonly IHostService _hostService;

    public HostController(IHostService hostService)
    {
        _hostService = hostService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HostDto>>> GetAllHosts()
    {
        var hosts = await _hostService.GetAllHostsAsync();
        return Ok(hosts);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HostDto>> GetHostById(int id)
    {
        try
        {
            var host = await _hostService.GetHostByIdAsync(id);
            return Ok(host);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<HostDto>> CreateHost(CreateHostDto request)
    {
        try
        {
            var host = await _hostService.CreateHostAsync(request);
            return CreatedAtAction(nameof(GetHostById), new { id = host.Id }, host);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<HostDto>> UpdateHost(int id, UpdateHostDto request)
    {
        try
        {
            var host = await _hostService.UpdateHostAsync(id, request);
            return Ok(host);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteHost(int id)
    {
        var deleted = await _hostService.DeleteHostAsync(id);

        if (!deleted)
        {
            return NotFound(new { message = $"Host with id {id} was not found." });
        }

        return NoContent();
    }

    [HttpGet("{id:int}/active")]
    public async Task<ActionResult<bool>> IsHostActive(int id)
    {
        try
        {
            var isActive = await _hostService.IsHostActiveAsync(id);
            return Ok(isActive);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
