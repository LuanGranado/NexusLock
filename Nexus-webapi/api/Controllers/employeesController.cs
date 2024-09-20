using Microsoft.AspNetCore.Mvc;
using Nexus_webapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Nexus_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly NexusDbContext _context;

        public EmployeesController(NexusDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all employees with optional pagination.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "ViewRooms")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var employees = await _context.Employees
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    Name = e.Name,
                    FingerprintDataBase64 = e.FingerprintData != null ? Convert.ToBase64String(e.FingerprintData) : null
                })
                .ToListAsync();

            return Ok(employees);
        }

        /// <summary>
        /// Retrieves a specific employee by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id)
        {
            var employee = await _context.Employees
                .Where(e => e.EmployeeId == id)
                .Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    Name = e.Name,
                    FingerprintDataBase64 = e.FingerprintData != null ? Convert.ToBase64String(e.FingerprintData) : null
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> PostEmployee(CreateEmployeeDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = new Employees
            {
                Name = createDto.Name,
                PinCode = createDto.PinCode,
                FingerprintData = !string.IsNullOrEmpty(createDto.FingerprintDataBase64) 
                    ? Convert.FromBase64String(createDto.FingerprintDataBase64) 
                    : null
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var employeeDto = new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                FingerprintDataBase64 = employee.FingerprintData != null ? Convert.ToBase64String(employee.FingerprintData) : null
            };

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeId }, employeeDto);
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDto updateDto)
        {
            if (id != updateDto.EmployeeId)
            {
                return BadRequest("Employee ID mismatch.");
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = updateDto.Name;
            employee.PinCode = updateDto.PinCode;
            employee.FingerprintData = !string.IsNullOrEmpty(updateDto.FingerprintDataBase64)
                ? Convert.FromBase64String(updateDto.FingerprintDataBase64)
                : employee.FingerprintData;

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id) => _context.Employees.Any(e => e.EmployeeId == id);

        [HttpPut("{id}/fingerprint")]
        public async Task<IActionResult> UpdateEmployeeFingerprint(int id, [FromBody] UpdateFingerprintDto updateDto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.FingerprintData = Convert.FromBase64String(updateDto.FingerprintDataBase64);
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        public class UpdateFingerprintDto
        {
            [Required]
            public string FingerprintDataBase64 { get; set; }
        }
    }

    // DTOs
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string? FingerprintDataBase64 { get; set; }
    }

    public class CreateEmployeeDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(10)]
        public string? PinCode { get; set; }

        public string? FingerprintDataBase64 { get; set; }
    }

    public class UpdateEmployeeDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(10)]
        public string? PinCode { get; set; }

        public string? FingerprintDataBase64 { get; set; }
    }
}