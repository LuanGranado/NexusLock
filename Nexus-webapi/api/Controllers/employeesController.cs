using Microsoft.AspNetCore.Mvc;
using Nexus_webapi.DAO;
using Nexus_webapi.Models;

namespace Nexus_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeesDAO _employeesDAO;

    public EmployeesController(EmployeesDAO employeesDAO)
    {
        _employeesDAO = employeesDAO;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employees>>> GetAllEmployees()
    {
        var employees = await _employeesDAO.GetAllEmployees();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employees>> GetEmployeeById(int id)
    {
        var employee = await _employeesDAO.GetEmployeeById(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateEmployee(Employees employee)
    {
        var id = await _employeesDAO.InsertEmployee(employee);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, Employees employee)
    {
        if (id != employee.EmployeeId)
        {
            return BadRequest();
        }

        var result = await _employeesDAO.UpdateEmployee(employee);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var result = await _employeesDAO.DeleteEmployee(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
