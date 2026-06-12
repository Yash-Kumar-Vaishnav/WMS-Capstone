using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class LeaveRepository : GenericRepository<Leave>, ILeaveRepository
{
    public LeaveRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Leave>> GetByEmployeeAsync(int empId)
        => await _context.Leaves.Where(x => x.EmpId == empId).ToListAsync();
}
