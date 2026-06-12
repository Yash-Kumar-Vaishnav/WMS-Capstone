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
