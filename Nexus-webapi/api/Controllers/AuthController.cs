using Microsoft.AspNetCore.Mvc;
using Nexus_webapi.Models;
using Nexus_webapi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
namespace Nexus_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly NexusDbContext _context;
        private readonly IAuthService _authService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(NexusDbContext context, IAuthService authService, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _authService = authService;
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Name == model.Username);

            if (employee == null || model.Password != employee.PinCode)
                return Unauthorized("Invalid credentials.");

            var token = await _authService.GenerateTokenAsync(employee);
            var userToken = new UserToken
            {
                EmployeeId = employee.EmployeeId,
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenLifetimeMinutes)
            };

            _context.UserTokens.Add(userToken);
            await _context.SaveChangesAsync();

            return Ok(new { Token = token });
        }

        /// <summary>
        /// Logs out the user by invalidating the JWT token.
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
                return BadRequest("No token provided.");

            var userToken = await _context.UserTokens.FirstOrDefaultAsync(ut => ut.Token == token);
            if (userToken != null)
            {
                _context.UserTokens.Remove(userToken);
                await _context.SaveChangesAsync();
            }

            return Ok("Logged out successfully.");
        }
    }
}
