using Microsoft.AspNetCore.Mvc;
using Nexus_webapi.DAO;
using Nexus_webapi.Models;

namespace Nexus_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeRoomAccessController : ControllerBase
{
    private readonly EmployeeRoomAccessDAO _employeeRoomAccessDAO;

    public EmployeeRoomAccessController(EmployeeRoomAccessDAO employeeRoomAccessDAO)
    {
        _employeeRoomAccessDAO = employeeRoomAccessDAO;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeRoomAccess>>> GetAllEmployeeRoomAccess()
    {
        var accesses = await _employeeRoomAccessDAO.GetAllEmployeeRoomAccess();
        return Ok(accesses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeRoomAccess>> GetEmployeeRoomAccessById(int id)
    {
        var access = await _employeeRoomAccessDAO.GetEmployeeRoomAccessById(id);
        if (access == null)
        {
            return NotFound();
        }
        return Ok(access);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateEmployeeRoomAccess(EmployeeRoomAccess access)
    {
        var id = await _employeeRoomAccessDAO.InsertEmployeeRoomAccess(access);
        return CreatedAtAction(nameof(GetEmployeeRoomAccessById), new { id = id }, id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeRoomAccess(int id)
    {
        var result = await _employeeRoomAccessDAO.DeleteEmployeeRoomAccess(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}