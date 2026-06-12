using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Project;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
        => _projectService = projectService;

    [HttpGet]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> GetAll()
        => Ok(await _projectService.GetAllAsync());

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _projectService.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var id = await _projectService.CreateAsync(dto);
        return Ok(new { ProjectId = id });
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromBody] UpdateProjectDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _projectService.UpdateAsync(dto);
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _projectService.DeleteAsync(id);
        return result ? Ok() : NotFound();
    }
}
