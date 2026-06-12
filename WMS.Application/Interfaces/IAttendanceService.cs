using WMS.Application.DTOs.Attendance;

namespace WMS.Application.Interfaces;

public interface IAttendanceService
{
    Task<IEnumerable<AttendanceDto>> GetAllAsync();
    Task<AttendanceDto?> GetByIdAsync(int id);
    Task<IEnumerable<AttendanceDto>> GetByEmployeeAsync(int empId);
    Task<IEnumerable<AttendanceDto>> GetMonthlyAsync(int empId, int year, int month);
    Task<int> CheckInAsync(CheckInDto dto);
    Task<bool> CheckOutAsync(int attendanceId, CheckOutDto dto);
    Task<int> CreateAsync(CreateAttendanceDto dto);
    Task<bool> UpdateAsync(UpdateAttendanceDto dto);
    Task<bool> DeleteAsync(int id);
    Task<byte[]> GenerateTimesheetCsvAsync(int year, int month);
}
