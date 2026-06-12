using AutoMapper;
using WMS.Application.DTOs.EmployeeProjectAllocation;
using WMS.Domain.Entities;

namespace WMS.Application.Mappings;

public class EmployeeProjectAllocationProfile : Profile
{
    public EmployeeProjectAllocationProfile()
    {
        CreateMap<EmployeeProjectAllocation, EmployeeProjectAllocationDto>();

        CreateMap<CreateEmployeeProjectAllocationDto,
            EmployeeProjectAllocation>();

        CreateMap<UpdateEmployeeProjectAllocationDto,
            EmployeeProjectAllocation>();
    }
}