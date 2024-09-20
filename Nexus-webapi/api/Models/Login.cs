using System.ComponentModel.DataAnnotations;

namespace Nexus_webapi.Models;
public class LoginModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}