using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Employee;
using System.Security.Claims;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
        => _employeeService = employeeService;

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll()
        => Ok(await _employeeService.GetAllAsync());

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> GetById(int id)
    {
        if (User.IsInRole("Employee") && User.FindFirstValue(ClaimTypes.NameIdentifier) != id.ToString())
        {
            return Forbid();
        }
        var result = await _employeeService.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Search([FromQuery] string? name,
        [FromQuery] int? departmentId, [FromQuery] int? roleId,
        [FromQuery] string? status)
        => Ok(await _employeeService.SearchAsync(name, departmentId, roleId, status));

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var id = await _employeeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, new { EmployeeId = id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromBody] UpdateEmployeeDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            return await _employeeService.UpdateAsync(dto) ? Ok() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
        => await _employeeService.DeleteAsync(id) ? Ok() : NotFound();
}
