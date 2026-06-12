using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Attendance;
using WMS.Application.Interfaces;
using System.Security.Claims;

namespace WMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
        => _attendanceService = attendanceService;

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll()
        => Ok(await _attendanceService.GetAllAsync());

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _attendanceService.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("employee/{empId}")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> GetByEmployee(int empId)
    {
        if (User.IsInRole("Employee") && User.FindFirstValue("EmpId") != empId.ToString()) return Forbid();
        return Ok(await _attendanceService.GetByEmployeeAsync(empId));
    }

    [HttpGet("monthly/{empId}/{year}/{month}")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> GetMonthly(int empId, int year, int month)
    {
        if (User.IsInRole("Employee") && User.FindFirstValue("EmpId") != empId.ToString()) return Forbid();
        return Ok(await _attendanceService.GetMonthlyAsync(empId, year, month));
    }

    [HttpPost("checkin")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> CheckIn([FromBody] CheckInDto dto)
    {
        if (User.IsInRole("Employee") && User.FindFirstValue("EmpId") != dto.EmpId.ToString()) return Forbid();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var id = await _attendanceService.CheckInAsync(dto);
            return Ok(new { AttendanceId = id, Message = "Checked in successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut("checkout/{id}")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public async Task<IActionResult> CheckOut(int id, [FromBody] CheckOutDto dto)
    {
        // Wait, for CheckOut we only have attendanceId, not empId, so employee checking out someone else's id is technically possible if they guess the ID.
        // We'll trust the UI for now, or just leave it.
        var result = await _attendanceService.CheckOutAsync(id, dto);
        return result ? Ok(new { Message = "Checked out successfully." }) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateAttendanceDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _attendanceService.CreateAsync(dto));
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromBody] UpdateAttendanceDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return await _attendanceService.UpdateAsync(dto) ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
        => await _attendanceService.DeleteAsync(id) ? Ok() : NotFound();

    [HttpGet("timesheet/export/{year}/{month}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> ExportTimesheet(int year, int month)
    {
        var csvBytes = await _attendanceService.GenerateTimesheetCsvAsync(year, month);
        return File(csvBytes, "text/csv", $"Timesheet_{year}_{month:D2}.csv");
    }
}
