using Microsoft.AspNetCore.Mvc;
using Nexus_webapi.DAO;
using Nexus_webapi.Models;

namespace Nexus_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolePermissionsController : ControllerBase
{
    private readonly RolePermissionsDAO _rolePermissionsDAO;

    public RolePermissionsController(RolePermissionsDAO rolePermissionsDAO)
    {
        _rolePermissionsDAO = rolePermissionsDAO;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RolePermissions>>> GetAllRolePermissions()
    {
        var rolePermissions = await _rolePermissionsDAO.GetAllRolePermissions();
        return Ok(rolePermissions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RolePermissions>> GetRolePermissionById(int id)
    {
        var rolePermission = await _rolePermissionsDAO.GetRolePermissionById(id);
        if (rolePermission == null)
        {
            return NotFound();
        }
        return Ok(rolePermission);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateRolePermission(RolePermissions rolePermission)
    {
        var id = await _rolePermissionsDAO.InsertRolePermission(rolePermission);
        return CreatedAtAction(nameof(GetRolePermissionById), new { id = id }, id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRolePermission(int id)
    {
        var result = await _rolePermissionsDAO.DeleteRolePermission(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
