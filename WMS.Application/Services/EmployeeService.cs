using AutoMapper;
using WMS.Application.DTOs.Employee;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;

namespace WMS.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        => _mapper.Map<IEnumerable<EmployeeDto>>(await _employeeRepository.GetAllAsync());

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var e = await _employeeRepository.GetByIdAsync(id);
        return e == null ? null : _mapper.Map<EmployeeDto>(e);
    }

    public async Task<IEnumerable<EmployeeDto>> SearchAsync(string? name, int? departmentId, int? roleId, string? status)
        => _mapper.Map<IEnumerable<EmployeeDto>>(
            await _employeeRepository.SearchAsync(name, departmentId, roleId, status));

    public async Task<int> CreateAsync(CreateEmployeeDto dto)
    {
        if (!await _employeeRepository.IsEmailUniqueAsync(dto.Email))
            throw new ArgumentException("Email is already in use.");

        var employee = _mapper.Map<Employee>(dto);
        await _employeeRepository.AddAsync(employee);
        await _employeeRepository.SaveChangesAsync();
        return employee.EmployeeId;
    }

    public async Task<bool> UpdateAsync(UpdateEmployeeDto dto)
    {
        if (!await _employeeRepository.IsEmailUniqueAsync(dto.Email, dto.EmployeeId))
            throw new ArgumentException("Email is already in use.");

        var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
        if (employee == null) return false;
        _mapper.Map(dto, employee);
        _employeeRepository.Update(employee);
        await _employeeRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null) return false;
        _employeeRepository.Delete(employee);
        await _employeeRepository.SaveChangesAsync();
        return true;
    }
}
