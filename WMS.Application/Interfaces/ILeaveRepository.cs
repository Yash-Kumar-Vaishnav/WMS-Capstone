using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface ILeaveRepository : IGenericRepository<Leave>
{
    Task<IEnumerable<Leave>> GetByEmployeeAsync(int empId);
}
