using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class EmployeeProjectAllocationRepository
    : GenericRepository<EmployeeProjectAllocation>,
      IEmployeeProjectAllocationRepository
{
    public EmployeeProjectAllocationRepository(
        ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<EmployeeProjectAllocation>>
        GetByEmployeeAsync(int empId)
    {
        return await _context.EmployeeProjectAllocations
            .Where(x => x.EmpId == empId)
            .ToListAsync();
    }

    public async Task<IEnumerable<EmployeeProjectAllocation>>
        GetByProjectAsync(int projectId)
    {
        return await _context.EmployeeProjectAllocations
            .Where(x => x.ProjectId == projectId)
            .ToListAsync();
    }
}