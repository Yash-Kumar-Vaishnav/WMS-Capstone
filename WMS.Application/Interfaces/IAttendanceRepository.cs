using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface IAttendanceRepository : IGenericRepository<Attendance>
{
    Task<IEnumerable<Attendance>> GetByEmployeeAsync(int empId);
    Task<IEnumerable<Attendance>> GetMonthlyAsync(int empId, int year, int month);
    Task<Attendance?> GetOpenCheckInAsync(int empId, DateTime date);
    Task<bool> HasAttendanceTodayAsync(int empId, DateTime date);
}
