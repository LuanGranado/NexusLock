using Microsoft.AspNetCore.Mvc;
using Nexus_webapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Nexus_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly NexusDbContext _context;

        public RoomsController(NexusDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all rooms with optional pagination.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAllRooms([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var rooms = await _context.Rooms
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RoomDto
                {
                    RoomId = r.RoomId,
                    RoomName = r.RoomName,
                    RoomDescription = r.RoomDescription
                })
                .ToListAsync();

            return Ok(rooms);
        }

        /// <summary>
        /// Retrieves a specific room by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetRoomById(int id)
        {
            var room = await _context.Rooms
                .Where(r => r.RoomId == id)
                .Select(r => new RoomDto
                {
                    RoomId = r.RoomId,
                    RoomName = r.RoomName,
                    RoomDescription = r.RoomDescription
                })
                .FirstOrDefaultAsync();

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        /// <summary>
        /// Creates a new room.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RoomDto>> CreateRoom(CreateRoomDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var room = new Rooms
            {
                RoomName = createDto.RoomName,
                RoomDescription = createDto.RoomDescription
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            var roomDto = new RoomDto
            {
                RoomId = room.RoomId,
                RoomName = room.RoomName,
                RoomDescription = room.RoomDescription
            };

            return CreatedAtAction(nameof(GetRoomById), new { id = room.RoomId }, roomDto);
        }

        /// <summary>
        /// Updates an existing room.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, UpdateRoomDto updateDto)
        {
            if (id != updateDto.RoomId)
            {
                return BadRequest("Room ID mismatch.");
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            room.RoomName = updateDto.RoomName;
            room.RoomDescription = updateDto.RoomDescription;

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a room by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomExists(int id) => _context.Rooms.Any(r => r.RoomId == id);

        [HttpGet("{id}/employees")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesWithAccessToRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            var employees = await _context.EmployeeRoomAccesses
                .Where(era => era.RoomId == id)
                .Include(era => era.Employee)
                .Select(era => new EmployeeDto
                {
                    EmployeeId = era.Employee.EmployeeId,
                    Name = era.Employee.Name,
                    FingerprintDataBase64 = era.Employee.FingerprintData != null ? Convert.ToBase64String(era.Employee.FingerprintData) : null
                })
                .ToListAsync();

            return Ok(employees);
        }
    }

    // DTOs
    public class RoomDto
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string? RoomDescription { get; set; }
    }

    public class CreateRoomDto
    {
        [Required]
        [StringLength(100)]
        public string RoomName { get; set; }

        public string? RoomDescription { get; set; }
    }

    public class UpdateRoomDto
    {
        [Required]
        public int RoomId { get; set; }

        [Required]
        [StringLength(100)]
        public string RoomName { get; set; }

        public string? RoomDescription { get; set; }
    }
}