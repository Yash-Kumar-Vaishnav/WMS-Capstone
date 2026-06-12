using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WMS.Application.DTOs.Announcement;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;

    public AnnouncementController(IAnnouncementService announcementService)
        => _announcementService = announcementService;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _announcementService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("active")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _announcementService.GetActiveAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _announcementService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateAnnouncementDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdStr, out var userId))
        {
            dto.CreatedBy = userId;
        }

        var id = await _announcementService.CreateAsync(dto);
        return Ok(new { AnnouncementId = id });
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromBody] UpdateAnnouncementDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _announcementService.UpdateAsync(dto);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _announcementService.DeleteAsync(id);
        if (!result) return NotFound();
        return Ok();
    }
}
