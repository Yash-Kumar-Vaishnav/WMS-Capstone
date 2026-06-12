using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface IEmployeeProjectAllocationRepository
    : IGenericRepository<EmployeeProjectAllocation>
{
    Task<IEnumerable<EmployeeProjectAllocation>>
        GetByEmployeeAsync(int empId);

    Task<IEnumerable<EmployeeProjectAllocation>>
        GetByProjectAsync(int projectId);
}