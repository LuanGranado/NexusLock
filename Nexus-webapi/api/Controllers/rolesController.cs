using Microsoft.AspNetCore.Mvc;
using Nexus_webapi.DAO;
using Nexus_webapi.Models;

namespace Nexus_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly RolesDAO _rolesDAO;

    public RolesController(RolesDAO rolesDAO)
    {
        _rolesDAO = rolesDAO;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Roles>>> GetAllRoles()
    {
        var roles = await _rolesDAO.GetAllRoles();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Roles>> GetRoleById(int id)
    {
        var role = await _rolesDAO.GetRoleById(id);
        if (role == null)
        {
            return NotFound();
        }
        return Ok(role);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateRole(Roles role)
    {
        var id = await _rolesDAO.InsertRole(role);
        return CreatedAtAction(nameof(GetRoleById), new { id = id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int id, Roles role)
    {
        if (id != role.RoleId)
        {
            return BadRequest();
        }

        var result = await _rolesDAO.UpdateRole(role);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var result = await _rolesDAO.DeleteRole(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}