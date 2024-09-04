using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_webapi.Models;

public class Employees
{
    [Key]
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Column("pin_code")]
    public string PinCode { get; set; }

    [Column("fingerprint_data")]
    public byte[] FingerprintData { get; set; }
}
