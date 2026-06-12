using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.EmployeeProjectAllocation;
using WMS.Application.Interfaces;
using System.Security.Claims;

namespace WMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeeProjectAllocationController : ControllerBase
{
    private readonly IEmployeeProjectAllocationService _service;

    public EmployeeProjectAllocationController(IEmployeeProjectAllocationService service)
        => _service = service;

    [HttpGet]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result != null && User.IsInRole("Employee") && User.FindFirstValue(ClaimTypes.NameIdentifier) != result.EmpId.ToString()) return Forbid();
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("project/{projectId}")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> GetByProject(int projectId)
    {
        return Ok(await _service.GetByProjectAsync(projectId));
    }

    [HttpGet("employee/{empId}")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> GetByEmployee(int empId)
    {
        if (User.IsInRole("Employee") && User.FindFirstValue(ClaimTypes.NameIdentifier) != empId.ToString()) return Forbid();
        return Ok(await _service.GetByEmployeeAsync(empId));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeProjectAllocationDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var id = await _service.CreateAsync(dto);
        return Ok(new { AllocationId = id });
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update([FromBody] UpdateEmployeeProjectAllocationDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _service.UpdateAsync(dto);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return Ok();
    }
}
