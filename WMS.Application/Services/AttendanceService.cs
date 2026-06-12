using AutoMapper;
using WMS.Application.DTOs.Attendance;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;

namespace WMS.Application.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _repo;
    private readonly IMapper _mapper;

    public AttendanceService(IAttendanceRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AttendanceDto>> GetAllAsync()
        => _mapper.Map<IEnumerable<AttendanceDto>>(await _repo.GetAllAsync());

    public async Task<AttendanceDto?> GetByIdAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id);
        return a == null ? null : _mapper.Map<AttendanceDto>(a);
    }

    public async Task<IEnumerable<AttendanceDto>> GetByEmployeeAsync(int empId)
        => _mapper.Map<IEnumerable<AttendanceDto>>(await _repo.GetByEmployeeAsync(empId));

    public async Task<IEnumerable<AttendanceDto>> GetMonthlyAsync(int empId, int year, int month)
        => _mapper.Map<IEnumerable<AttendanceDto>>(await _repo.GetMonthlyAsync(empId, year, month));

    public async Task<int> CheckInAsync(CheckInDto dto)
    {
        var today = DateTime.Today;
        if (await _repo.HasAttendanceTodayAsync(dto.EmpId, today))
            throw new InvalidOperationException("Employee has already checked in today.");

        var attendance = new Attendance
        {
            EmpId = dto.EmpId,
            CheckIn = DateTime.Now,
            AttendanceDate = today,
            WorkMode = dto.WorkMode
        };
        await _repo.AddAsync(attendance);
        await _repo.SaveChangesAsync();
        return attendance.AttendanceId;
    }

    public async Task<bool> CheckOutAsync(int attendanceId, CheckOutDto dto)
    {
        var attendance = await _repo.GetByIdAsync(attendanceId);
        if (attendance == null || attendance.CheckOut.HasValue) return false;
        
        attendance.CheckOut = DateTime.Now;
        attendance.TotalHours = Math.Max(0, (attendance.CheckOut.Value - attendance.CheckIn).TotalHours);
        
        _repo.Update(attendance);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<int> CreateAsync(CreateAttendanceDto dto)
    {
        var attendance = _mapper.Map<Attendance>(dto);
        if (attendance.CheckOut.HasValue)
            attendance.TotalHours = (attendance.CheckOut.Value - attendance.CheckIn).TotalHours;
        await _repo.AddAsync(attendance);
        await _repo.SaveChangesAsync();
        return attendance.AttendanceId;
    }

    public async Task<bool> UpdateAsync(UpdateAttendanceDto dto)
    {
        var attendance = await _repo.GetByIdAsync(dto.AttendanceId);
        if (attendance == null) return false;
        _mapper.Map(dto, attendance);
        if (attendance.CheckOut.HasValue)
            attendance.TotalHours = (attendance.CheckOut.Value - attendance.CheckIn).TotalHours;
        _repo.Update(attendance);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id);
        if (a == null) return false;
        _repo.Delete(a);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<byte[]> GenerateTimesheetCsvAsync(int year, int month)
    {
        var all = await _repo.GetAllAsync();
        var monthly = all.Where(a => a.AttendanceDate.Year == year && a.AttendanceDate.Month == month).ToList();

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("AttendanceId,EmpId,AttendanceDate,CheckIn,CheckOut,TotalHours,WorkMode");
        foreach (var a in monthly)
        {
            sb.AppendLine($"{a.AttendanceId},{a.EmpId},{a.AttendanceDate:yyyy-MM-dd},{a.CheckIn:HH:mm},{a.CheckOut?.ToString("HH:mm") ?? ""},{a.TotalHours},{a.WorkMode}");
        }
        return System.Text.Encoding.UTF8.GetBytes(sb.ToString());
    }
}
