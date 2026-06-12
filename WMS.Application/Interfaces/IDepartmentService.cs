using WMS.Application.DTOs.Department;

namespace WMS.Application.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateDepartmentDto dto);
    Task<bool> UpdateAsync(UpdateDepartmentDto dto);
    Task<bool> DeleteAsync(int id);
}
