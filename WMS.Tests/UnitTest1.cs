using Moq;
using AutoMapper;
using WMS.Application.DTOs.Employee;
using WMS.Application.DTOs.Leave;
using WMS.Application.DTOs.Attendance;
using WMS.Application.Interfaces;
using WMS.Application.Services;
using WMS.Domain.Entities;

namespace WMS.Tests;

// ─── Employee Service Tests ────────────────────────────────────────────────────
public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _repoMock   = new();
    private readonly Mock<IMapper>             _mapperMock = new();
    private          EmployeeService           Service()   =>
        new(_repoMock.Object, _mapperMock.Object);

    [Fact]
    public async Task GetAllAsync_ReturnsAllEmployees()
    {
        var entities = new List<Employee>
        {
            new() { EmployeeId = 1, FirstName = "Alice", LastName = "Smith" },
            new() { EmployeeId = 2, FirstName = "Bob",   LastName = "Jones" }
        };
        var dtos = entities.Select(e => new EmployeeDto
            { EmployeeId = e.EmployeeId, FirstName = e.FirstName, LastName = e.LastName }).ToList();

        _repoMock  .Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
        _mapperMock.Setup(m => m.Map<IEnumerable<EmployeeDto>>(entities)).Returns(dtos);

        var result = await Service().GetAllAsync();
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEmployee_WhenFound()
    {
        var entity = new Employee { EmployeeId = 1, FirstName = "Alice" };
        var dto    = new EmployeeDto { EmployeeId = 1, FirstName = "Alice" };
        _repoMock  .Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<EmployeeDto>(entity)).Returns(dto);
        var result = await Service().GetByIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal(1, result!.EmployeeId);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Employee?)null);
        var result = await Service().GetByIdAsync(99);
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ReturnsNewId()
    {
        var dto    = new CreateEmployeeDto
        {
            FirstName = "Test", LastName = "User", Email = "test@wms.com",
            PhoneNumber = "9876543210", Gender = "M",
            DOB = DateTime.Today.AddYears(-25), DOJ = DateTime.Today,
            DepartmentId = 1, RoleId = 3
        };
        var entity = new Employee { EmployeeId = 42 };
        _repoMock.Setup(r => r.IsEmailUniqueAsync(It.IsAny<string>(), null)).ReturnsAsync(true);
        _mapperMock.Setup(m => m.Map<Employee>(dto)).Returns(entity);
        _repoMock  .Setup(r => r.AddAsync(entity)).Returns(Task.CompletedTask);
        _repoMock  .Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        var id = await Service().CreateAsync(dto);
        Assert.Equal(42, id);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Employee?)null);
        var result = await Service().DeleteAsync(99);
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
    {
        var dto = new UpdateEmployeeDto
        {
            EmployeeId = 99, FirstName = "X", LastName = "Y",
            Email = "x@y.com", PhoneNumber = "0000000000", Gender = "M",
            DOB = DateTime.Today.AddYears(-20), DOJ = DateTime.Today,
            DepartmentId = 1, RoleId = 3
        };
        _repoMock.Setup(r => r.IsEmailUniqueAsync(It.IsAny<string>(), It.IsAny<int?>())).ReturnsAsync(true);
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Employee?)null);
        var result = await Service().UpdateAsync(dto);
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_ReturnsFilteredEmployees()
    {
        var entities = new List<Employee>
        {
            new() { EmployeeId = 1, FirstName = "Alice", LastName = "Smith", DepartmentId = 1 }
        };
        var dtos = entities.Select(e => new EmployeeDto { EmployeeId = e.EmployeeId, FirstName = e.FirstName }).ToList();
        _repoMock  .Setup(r => r.SearchAsync("Alice", null, null, null)).ReturnsAsync(entities);
        _mapperMock.Setup(m => m.Map<IEnumerable<EmployeeDto>>(entities)).Returns(dtos);
        var result = await Service().SearchAsync("Alice", null, null, null);
        Assert.Single(result);
    }
}

// ─── Leave Service Tests ───────────────────────────────────────────────────────
public class LeaveServiceTests
{
    private readonly Mock<ILeaveRepository> _repoMock   = new();
    private readonly Mock<IMapper>          _mapperMock = new();
    private          LeaveService           Service()   =>
        new(_repoMock.Object, _mapperMock.Object);

    [Fact]
    public async Task GetAllAsync_ReturnsAllLeaves()
    {
        var entities = new List<Leave>
        {
            new() { LeaveId = 1, EmpId = 1, LeaveType = "Sick",   Status = "Pending" },
            new() { LeaveId = 2, EmpId = 2, LeaveType = "Casual", Status = "Approved" }
        };
        var dtos = entities.Select(e => new LeaveDto
            { LeaveId = e.LeaveId, EmpId = e.EmpId, LeaveType = e.LeaveType, Status = e.Status }).ToList();
        _repoMock  .Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
        _mapperMock.Setup(m => m.Map<IEnumerable<LeaveDto>>(entities)).Returns(dtos);
        var result = await Service().GetAllAsync();
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ApproveAsync_ReturnsTrue_WhenLeaveExists()
    {
        var leave = new Leave { LeaveId = 1, EmpId = 1, Status = "Pending" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(leave);
        _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        var result = await Service().ApproveAsync(1, 2);
        Assert.True(result);
        Assert.Equal("Approved", leave.Status);
        Assert.Equal(2, leave.ApprovedBy);
    }

    [Fact]
    public async Task RejectAsync_ReturnsTrue_WhenLeaveExists()
    {
        var leave = new Leave { LeaveId = 1, EmpId = 1, Status = "Pending" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(leave);
        _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        var result = await Service().RejectAsync(1, 2);
        Assert.True(result);
        Assert.Equal("Rejected", leave.Status);
    }

    [Fact]
    public async Task CancelAsync_ReturnsFalse_WhenAlreadyApproved()
    {
        var leave = new Leave { LeaveId = 1, Status = "Approved" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(leave);
        var result = await Service().CancelAsync(1);
        Assert.False(result);
    }

    [Fact]
    public async Task CancelAsync_ReturnsTrue_WhenPending()
    {
        var leave = new Leave { LeaveId = 1, Status = "Pending" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(leave);
        _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        var result = await Service().CancelAsync(1);
        Assert.True(result);
        Assert.Equal("Cancelled", leave.Status);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenLeaveNotFound()
    {
        var dto = new UpdateLeaveDto { LeaveId = 99, EmpId = 1, LeaveType = "Sick",
            FromDate = DateTime.Today, ToDate = DateTime.Today.AddDays(1) };
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Leave?)null);
        var result = await Service().UpdateAsync(dto);
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Leave?)null);
        var result = await Service().DeleteAsync(99);
        Assert.False(result);
    }
}

// ─── Attendance Service Tests ──────────────────────────────────────────────────
public class AttendanceServiceTests
{
    private readonly Mock<IAttendanceRepository> _repoMock   = new();
    private readonly Mock<IMapper>               _mapperMock = new();
    private          AttendanceService           Service()   =>
        new(_repoMock.Object, _mapperMock.Object);

    [Fact]
    public async Task GetAllAsync_ReturnsAttendanceList()
    {
        var entities = new List<Attendance>
        {
            new() { AttendanceId = 1, EmpId = 1, CheckIn = DateTime.Now, AttendanceDate = DateTime.Today }
        };
        var dtos = new List<AttendanceDto>
        {
            new() { AttendanceId = 1, EmpId = 1, AttendanceDate = DateTime.Today }
        };
        _repoMock  .Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
        _mapperMock.Setup(m => m.Map<IEnumerable<AttendanceDto>>(entities)).Returns(dtos);
        var result = await Service().GetAllAsync();
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Attendance?)null);
        var result = await Service().GetByIdAsync(99);
        Assert.Null(result);
    }

    [Fact]
    public async Task CheckInAsync_ReturnsId()
    {
        var dto = new CheckInDto { EmpId = 1, CheckIn = DateTime.Now, AttendanceDate = DateTime.Today, WorkMode = "WFO" };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Attendance>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        var id = await Service().CheckInAsync(dto);
        Assert.Equal(0, id); // 0 because no DB to assign ID
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Attendance>()), Times.Once);
    }

    [Fact]
    public async Task CheckOutAsync_ReturnsFalse_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Attendance?)null);
        var result = await Service().CheckOutAsync(99, new CheckOutDto { CheckOut = DateTime.Now });
        Assert.False(result);
    }

    [Fact]
    public async Task CheckOutAsync_ReturnsTrue_AndCalculatesHours()
    {
        var checkIn = DateTime.Now.AddHours(-8);
        var attendance = new Attendance { AttendanceId = 1, EmpId = 1, CheckIn = checkIn, AttendanceDate = DateTime.Today };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(attendance);
        _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        var checkOut = DateTime.Now;
        var result = await Service().CheckOutAsync(1, new CheckOutDto { CheckOut = checkOut });
        Assert.True(result);
        Assert.NotNull(attendance.CheckOut);
        Assert.True(attendance.TotalHours >= 7.9);
    }
}
