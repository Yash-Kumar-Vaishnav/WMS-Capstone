using AutoMapper;
using WMS.Application.DTOs.Leave;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;

namespace WMS.Application.Services;

public class LeaveService : ILeaveService
{
    private readonly ILeaveRepository _repo;
    private readonly IMapper _mapper;

    public LeaveService(ILeaveRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LeaveDto>> GetAllAsync()
        => _mapper.Map<IEnumerable<LeaveDto>>(await _repo.GetAllAsync());

    public async Task<LeaveDto?> GetByIdAsync(int id)
    {
        var l = await _repo.GetByIdAsync(id);
        return l == null ? null : _mapper.Map<LeaveDto>(l);
    }

    public async Task<IEnumerable<LeaveDto>> GetByEmployeeAsync(int empId)
        => _mapper.Map<IEnumerable<LeaveDto>>(await _repo.GetByEmployeeAsync(empId));

    public async Task<int> CreateAsync(CreateLeaveDto dto)
    {
        var leave = _mapper.Map<Leave>(dto);
        await _repo.AddAsync(leave);
        await _repo.SaveChangesAsync();
        return leave.LeaveId;
    }

    public async Task<bool> UpdateAsync(UpdateLeaveDto dto)
    {
        var leave = await _repo.GetByIdAsync(dto.LeaveId);
        if (leave == null) return false;
        _mapper.Map(dto, leave);
        _repo.Update(leave);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var leave = await _repo.GetByIdAsync(id);
        if (leave == null) return false;
        _repo.Delete(leave);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ApproveAsync(int leaveId, int approverId)
    {
        var leave = await _repo.GetByIdAsync(leaveId);
        if (leave == null) return false;
        leave.Status = "Approved";
        leave.ApprovedBy = approverId;
        leave.ApprovedOn = DateTime.Now;
        _repo.Update(leave);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectAsync(int leaveId, int approverId)
    {
        var leave = await _repo.GetByIdAsync(leaveId);
        if (leave == null) return false;
        leave.Status = "Rejected";
        leave.ApprovedBy = approverId;
        leave.ApprovedOn = DateTime.Now;
        _repo.Update(leave);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelAsync(int leaveId)
    {
        var leave = await _repo.GetByIdAsync(leaveId);
        if (leave == null) return false;
        if (leave.Status != "Pending") return false; // Cannot cancel unless Pending
        leave.Status = "Cancelled";
        _repo.Update(leave);
        await _repo.SaveChangesAsync();
        return true;
    }
}
