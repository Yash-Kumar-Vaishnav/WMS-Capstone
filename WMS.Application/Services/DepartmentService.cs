using AutoMapper;
using WMS.Application.DTOs.Department;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;

namespace WMS.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repo;
    private readonly IMapper               _mapper;

    public DepartmentService(IDepartmentRepository repo, IMapper mapper)
    {
        _repo   = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        var dept = await _repo.GetByIdAsync(id);
        return dept == null ? null : _mapper.Map<DepartmentDto>(dept);
    }

    public async Task<int> CreateAsync(CreateDepartmentDto dto)
    {
        var existing = await _repo.GetByNameAsync(dto.DepartmentName);
        if (existing != null)
            throw new ArgumentException("Department name already exists.");

        var dept = _mapper.Map<Department>(dto);
        await _repo.AddAsync(dept);
        await _repo.SaveChangesAsync();
        return dept.DepartmentId;
    }

    public async Task<bool> UpdateAsync(UpdateDepartmentDto dto)
    {
        var existing = await _repo.GetByNameAsync(dto.DepartmentName);
        if (existing != null && existing.DepartmentId != dto.DepartmentId)
            throw new ArgumentException("Department name already exists.");

        var dept = await _repo.GetByIdAsync(dto.DepartmentId);
        if (dept == null) return false;
        _mapper.Map(dto, dept);
        dept.UpdatedOn = DateTime.Now;
        _repo.Update(dept);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var dept = await _repo.GetByIdAsync(id);
        if (dept == null) return false;
        _repo.Delete(dept);
        await _repo.SaveChangesAsync();
        return true;
    }
}
