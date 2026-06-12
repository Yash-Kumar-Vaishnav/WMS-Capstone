using AutoMapper;
using WMS.Application.DTOs.Employee;
using WMS.Domain.Entities;

namespace WMS.Application.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department != null ? s.Department.DepartmentName : null))
            .ForMember(d => d.RoleName,       o => o.MapFrom(s => s.Role != null ? s.Role.RoleName : null));

        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();
    }
}
