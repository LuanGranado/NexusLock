using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_webapi.Models;

public class Permissions
{
    [Key]
    [Column("permission_id")]
    public int PermissionId { get; set; }

    [Required]
    [Column("permission_name")]
    public string PermissionName { get; set; }

    [Column("description")]
    public string Description { get; set; }
}
