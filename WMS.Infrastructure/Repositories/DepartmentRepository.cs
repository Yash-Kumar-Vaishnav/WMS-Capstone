using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Department?> GetByNameAsync(string name)
        => await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == name);
}
