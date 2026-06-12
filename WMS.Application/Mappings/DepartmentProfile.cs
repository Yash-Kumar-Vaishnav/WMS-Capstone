using AutoMapper;
using WMS.Application.DTOs.Department;
using WMS.Domain.Entities;

namespace WMS.Application.Mappings;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>()
            .ForMember(dest => dest.DepartmentId, opt => opt.Ignore());
    }
}
