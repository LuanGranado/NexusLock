using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_webapi.Models;

public class AcessLogs
{
    [Key]
    [Column("log_id")]
    public int IdLog { get; set; }

    [Column("employee_id")]
    public int IdEmployee { get; set; }

    [Column("room_id")]
    public int IdRoom { get; set; }

    [Column("access_time")]
    public DateTime AccessTime { get; set; }

    [Column("access_granted")]
    public bool AccessGranted { get; set; }

    [ForeignKey("IdEmployee")]
    public Employees Employee { get; set; }

    [ForeignKey("IdRoom")]
    public Rooms Room { get; set; }
}