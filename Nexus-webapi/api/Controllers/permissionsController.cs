using Microsoft.AspNetCore.Mvc;
using Nexus_webapi.DAO;
using Nexus_webapi.Models;

namespace Nexus_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly PermissionsDAO _permissionsDAO;

    public PermissionsController(PermissionsDAO permissionsDAO)
    {
        _permissionsDAO = permissionsDAO;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Permissions>>> GetAllPermissions()
    {
        var permissions = await _permissionsDAO.GetAllPermissions();
        return Ok(permissions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Permissions>> GetPermissionById(int id)
    {
        var permission = await _permissionsDAO.GetPermissionById(id);
        if (permission == null)
        {
            return NotFound();
        }
        return Ok(permission);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreatePermission(Permissions permission)
    {
        var id = await _permissionsDAO.InsertPermission(permission);
        return CreatedAtAction(nameof(GetPermissionById), new { id = id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePermission(int id, Permissions permission)
    {
        if (id != permission.PermissionId)
        {
            return BadRequest();
        }

        var result = await _permissionsDAO.UpdatePermission(permission);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePermission(int id)
    {
        var result = await _permissionsDAO.DeletePermission(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
