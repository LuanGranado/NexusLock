using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_webapi.Models;

public class Rooms
{
    [Key]
    [Column("room_id")]
    public int RoomId { get; set; }

    [Required]
    [Column("room_name")]
    [StringLength(100)]
    public string RoomName { get; set; }

    [Column("room_description")]
    public string? RoomDescription { get; set; }
}
