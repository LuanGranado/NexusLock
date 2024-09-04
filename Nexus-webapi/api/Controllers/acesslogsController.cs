using Microsoft.AspNetCore.Mvc;
using Nexus_webapi.DAO;
using Nexus_webapi.Models;

namespace Nexus_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccessLogsController : ControllerBase
{
    private readonly AccessLogsDAO _accessLogsDAO;

    public AccessLogsController(AccessLogsDAO accessLogsDAO)
    {
        _accessLogsDAO = accessLogsDAO;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccessLogs>>> GetAllAccessLogs()
    {
        var logs = await _accessLogsDAO.GetAllAccessLogs();
        return Ok(logs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccessLogs>> GetAccessLogById(int id)
    {
        var log = await _accessLogsDAO.GetAccessLogById(id);
        if (log == null)
        {
            return NotFound();
        }
        return Ok(log);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateAccessLog(AccessLogs log)
    {
        var id = await _accessLogsDAO.InsertAccessLog(log);
        return CreatedAtAction(nameof(GetAccessLogById), new { id = id }, id);
    }
}