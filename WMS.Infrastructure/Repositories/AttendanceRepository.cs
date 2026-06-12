using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Attendance>> GetByEmployeeAsync(int empId)
        => await _context.Attendances.Where(x => x.EmpId == empId).ToListAsync();

    public async Task<IEnumerable<Attendance>> GetMonthlyAsync(int empId, int year, int month)
        => await _context.Attendances
            .Where(x => x.EmpId == empId && x.AttendanceDate.Year == year && x.AttendanceDate.Month == month)
            .OrderBy(x => x.AttendanceDate)
            .ToListAsync();

    public async Task<Attendance?> GetOpenCheckInAsync(int empId, DateTime date)
        => await _context.Attendances
            .FirstOrDefaultAsync(x => x.EmpId == empId
                && x.AttendanceDate.Date == date.Date
                && x.CheckOut == null);

    public async Task<bool> HasAttendanceTodayAsync(int empId, DateTime date)
        => await _context.Attendances.AnyAsync(x => x.EmpId == empId && x.AttendanceDate.Date == date.Date);
}
