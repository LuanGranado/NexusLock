using Microsoft.AspNetCore.Mvc;
using Nexus_webapi.DAO;
using Nexus_webapi.Models;

namespace Nexus_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeRolesController : ControllerBase
{
    private readonly EmployeeRolesDAO _employeeRolesDAO;

    public EmployeeRolesController(EmployeeRolesDAO employeeRolesDAO)
    {
        _employeeRolesDAO = employeeRolesDAO;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeRoles>>> GetAllEmployeeRoles()
    {
        var employeeRoles = await _employeeRolesDAO.GetAllEmployeeRoles();
        return Ok(employeeRoles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeRoles>> GetEmployeeRoleById(int id)
    {
        var employeeRole = await _employeeRolesDAO.GetEmployeeRoleById(id);
        if (employeeRole == null)
        {
            return NotFound();
        }
        return Ok(employeeRole);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateEmployeeRole(EmployeeRoles employeeRole)
    {
        var id = await _employeeRolesDAO.InsertEmployeeRole(employeeRole);
        return CreatedAtAction(nameof(GetEmployeeRoleById), new { id = id }, id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeRole(int id)
    {
        var result = await _employeeRolesDAO.DeleteEmployeeRole(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
