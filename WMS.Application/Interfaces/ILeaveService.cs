using WMS.Application.DTOs.Leave;

namespace WMS.Application.Interfaces;

public interface ILeaveService
{
    Task<IEnumerable<LeaveDto>> GetAllAsync();
    Task<LeaveDto?> GetByIdAsync(int id);
    Task<IEnumerable<LeaveDto>> GetByEmployeeAsync(int empId);
    Task<int> CreateAsync(CreateLeaveDto dto);
    Task<bool> UpdateAsync(UpdateLeaveDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ApproveAsync(int leaveId, int approverId);
    Task<bool> RejectAsync(int leaveId, int approverId);
    Task<bool> CancelAsync(int leaveId);
}
