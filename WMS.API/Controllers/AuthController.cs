using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Auth;
using WMS.Application.Interfaces;
using WMS.Infrastructure.Persistence;

namespace WMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService        _tokenService;

    public AuthController(ApplicationDbContext context, ITokenService tokenService)
    {
        _context      = context;
        _tokenService = tokenService;
    }

    [HttpGet("test-db")]
    public async Task<IActionResult> TestDb()
    {
        try
        {
            var userCount = await _context.UserLogins.CountAsync();
            var allUsers = await _context.UserLogins.Select(u => new { u.Username, u.PasswordHash }).ToListAsync();
            return Ok(new { Message = "Database is connected!", UserCount = userCount, Users = allUsers });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message, Inner = ex.InnerException?.Message });
        }
    }

    /// <summary>Login — returns JWT token. Default: admin / Admin@123</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.UserLogins
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (user == null)
            return Unauthorized(new { message = "Invalid username or password." });

        bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
      
        if (!isValid)
            return Unauthorized(new { message = "Invalid username or password." });

        user.LastLogin = DateTime.Now;
        await _context.SaveChangesAsync();

        var token = _tokenService.GenerateToken(user, user.Role!.RoleName, user.UserId.ToString());

        return Ok(new LoginResponseDto
        {
            Token    = token,
            Username = user.Username,
            Role     = user.Role!.RoleName,
            EmpId    = user.UserId
        });
    }
}
