using AutoMapper;
using WMS.Application.DTOs.EmployeeProjectAllocation;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;

namespace WMS.Application.Services;

public class EmployeeProjectAllocationService : IEmployeeProjectAllocationService
{
    private readonly IEmployeeProjectAllocationRepository _repo;
    private readonly IMapper _mapper;

    public EmployeeProjectAllocationService(IEmployeeProjectAllocationRepository repo, IMapper mapper)
    {
        _repo   = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeProjectAllocationDto>> GetAllAsync()
        => _mapper.Map<IEnumerable<EmployeeProjectAllocationDto>>(await _repo.GetAllAsync());

    public async Task<EmployeeProjectAllocationDto?> GetByIdAsync(int id)
    {
        var e = await _repo.GetByIdAsync(id);
        return e == null ? null : _mapper.Map<EmployeeProjectAllocationDto>(e);
    }

    public async Task<IEnumerable<EmployeeProjectAllocationDto>> GetByProjectAsync(int projectId)
        => _mapper.Map<IEnumerable<EmployeeProjectAllocationDto>>(await _repo.GetByProjectAsync(projectId));

    public async Task<IEnumerable<EmployeeProjectAllocationDto>> GetByEmployeeAsync(int empId)
        => _mapper.Map<IEnumerable<EmployeeProjectAllocationDto>>(await _repo.GetByEmployeeAsync(empId));

    public async Task<int> CreateAsync(CreateEmployeeProjectAllocationDto dto)
    {
        if (dto.AssignedOn.Date < DateTime.Today)
            throw new ArgumentException("Project assignment date cannot be earlier than today.");

        var entity = _mapper.Map<EmployeeProjectAllocation>(dto);
        entity.CreateDate = DateTime.Now;
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return entity.AllocationId;
    }

    public async Task<bool> UpdateAsync(UpdateEmployeeProjectAllocationDto dto)
    {
        if (dto.AssignedOn.Date < DateTime.Today)
            throw new ArgumentException("Project assignment date cannot be earlier than today.");

        var entity = await _repo.GetByIdAsync(dto.AllocationId);
        if (entity == null) return false;
        _mapper.Map(dto, entity);
        entity.UpdatedDate = DateTime.Now;
        _repo.Update(entity);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        _repo.Delete(entity);
        await _repo.SaveChangesAsync();
        return true;
    }
}
