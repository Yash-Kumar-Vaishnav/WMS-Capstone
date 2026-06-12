using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context) { }

    public override async Task<IEnumerable<Employee>> GetAllAsync()
        => await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Role)
            .ToListAsync();

    public override async Task<Employee?> GetByIdAsync(int id)
        => await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Role)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);

    public async Task<IEnumerable<Employee>> SearchAsync(string? name, int? departmentId, int? roleId, string? status)
    {
        var query = _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(e => (e.FirstName + " " + e.LastName).Contains(name));

        if (departmentId.HasValue)
            query = query.Where(e => e.DepartmentId == departmentId.Value);

        if (roleId.HasValue)
            query = query.Where(e => e.RoleId == roleId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(e => e.Status == status);

        return await query.ToListAsync();
    }

    public async Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
    {
        var query = _context.Employees.Where(e => e.Email == email);
        if (excludeId.HasValue)
            query = query.Where(e => e.EmployeeId != excludeId.Value);
        return !await query.AnyAsync();
    }
}
