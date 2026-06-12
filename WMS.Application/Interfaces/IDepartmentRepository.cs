using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<Department?> GetByNameAsync(string name);
}
