using WMS.Application.DTOs.Employee;

namespace WMS.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<IEnumerable<EmployeeDto>> SearchAsync(string? name, int? departmentId, int? roleId, string? status);
    Task<int> CreateAsync(CreateEmployeeDto dto);
    Task<bool> UpdateAsync(UpdateEmployeeDto dto);
    Task<bool> DeleteAsync(int id);
}
