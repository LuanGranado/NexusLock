using Microsoft.AspNetCore.Mvc;
using Nexus_webapi.DAO;
using Nexus_webapi.Models;

namespace Nexus_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly RoomsDAO _roomsDAO;

    public RoomsController(RoomsDAO roomsDAO)
    {
        _roomsDAO = roomsDAO;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rooms>>> GetAllRooms()
    {
        var rooms = await _roomsDAO.GetAllRooms();
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Rooms>> GetRoomById(int id)
    {
        var room = await _roomsDAO.GetRoomById(id);
        if (room == null)
        {
            return NotFound();
        }
        return Ok(room);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateRoom(Rooms room)
    {
        var id = await _roomsDAO.InsertRoom(room);
        return CreatedAtAction(nameof(GetRoomById), new { id = id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(int id, Rooms room)
    {
        if (id != room.RoomId)
        {
            return BadRequest();
        }

        var result = await _roomsDAO.UpdateRoom(room);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var result = await _roomsDAO.DeleteRoom(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
