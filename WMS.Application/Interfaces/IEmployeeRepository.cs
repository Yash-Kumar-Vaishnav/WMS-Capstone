using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<IEnumerable<Employee>> SearchAsync(string? name, int? departmentId, int? roleId, string? status);
    Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
}
