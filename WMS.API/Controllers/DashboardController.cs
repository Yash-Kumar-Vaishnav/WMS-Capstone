using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WMS.Infrastructure.Persistence;

namespace WMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public DashboardController(ApplicationDbContext context) => _context = context;

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var today = DateTime.Today;
        var role = User.IsInRole("Employee") ? "Employee" : "Admin";
        var empIdStr = User.FindFirstValue("EmpId");
        int? empId = int.TryParse(empIdStr, out var id) ? id : null;

        if (role == "Employee" && empId.HasValue)
        {
            return Ok(new
            {
                TotalEmployees       = 0,
                TodayAttendanceCount = await _context.Attendances.CountAsync(a => a.EmpId == empId.Value && a.AttendanceDate.Date == today),
                PendingLeaveRequests = await _context.Leaves.CountAsync(l => l.EmpId == empId.Value && l.Status == "Pending"),
                ActiveProjects       = await _context.EmployeeProjectAllocations.Include(epa => epa.Project).CountAsync(epa => epa.EmpId == empId.Value && epa.Project!.Status == "Active"),
                TotalDepartments     = 0
            });
        }

        return Ok(new
        {
            TotalEmployees       = await _context.Employees.CountAsync(e => e.Status == "Active"),
            TodayAttendanceCount = await _context.Attendances.CountAsync(a => a.AttendanceDate.Date == today),
            PendingLeaveRequests = await _context.Leaves.CountAsync(l => l.Status == "Pending"),
            ActiveProjects       = await _context.Projects.CountAsync(p => p.Status == "Active"),
            TotalDepartments     = await _context.Departments.CountAsync()
        });
    }

    [HttpGet("leave-stats")]
    public async Task<IActionResult> GetLeaveStats()
    {
        var role = User.IsInRole("Employee") ? "Employee" : "Admin";
        var empIdStr = User.FindFirstValue("EmpId");
        int? empId = int.TryParse(empIdStr, out var id) ? id : null;

        var query = _context.Leaves.AsQueryable();
        if (role == "Employee" && empId.HasValue)
            query = query.Where(l => l.EmpId == empId.Value);

        var stats = await query
            .GroupBy(l => l.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();
        return Ok(stats);
    }

    [HttpGet("attendance-chart")]
    public async Task<IActionResult> GetAttendanceChart()
    {
        var role = User.IsInRole("Employee") ? "Employee" : "Admin";
        var empIdStr = User.FindFirstValue("EmpId");
        int? empId = int.TryParse(empIdStr, out var id) ? id : null;

        var from = DateTime.Today.AddDays(-29);
        var query = _context.Attendances.Where(a => a.AttendanceDate >= from);
        if (role == "Employee" && empId.HasValue)
            query = query.Where(a => a.EmpId == empId.Value);

        var data = await query
            .GroupBy(a => a.AttendanceDate.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .OrderBy(x => x.Date)
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("project-counts")]
    public async Task<IActionResult> GetProjectCounts()
    {
        return Ok(new {
            Active    = await _context.Projects.CountAsync(p => p.Status == "Active"),
            Completed = await _context.Projects.CountAsync(p => p.Status == "Completed")
        });
    }
}
