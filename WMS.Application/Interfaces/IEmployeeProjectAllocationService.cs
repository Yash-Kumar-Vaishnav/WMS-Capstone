using WMS.Application.DTOs.EmployeeProjectAllocation;

namespace WMS.Application.Interfaces;

public interface IEmployeeProjectAllocationService
{
    Task<IEnumerable<EmployeeProjectAllocationDto>> GetAllAsync();
    Task<EmployeeProjectAllocationDto?> GetByIdAsync(int id);
    Task<IEnumerable<EmployeeProjectAllocationDto>> GetByProjectAsync(int projectId);
    Task<IEnumerable<EmployeeProjectAllocationDto>> GetByEmployeeAsync(int empId);
    Task<int> CreateAsync(CreateEmployeeProjectAllocationDto dto);
    Task<bool> UpdateAsync(UpdateEmployeeProjectAllocationDto dto);
    Task<bool> DeleteAsync(int id);
}
