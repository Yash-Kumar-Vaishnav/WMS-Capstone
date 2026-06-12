using AutoMapper;
using WMS.Application.DTOs.Leave;
using WMS.Domain.Entities;

namespace WMS.Application.Mappings;

public class LeaveProfile : Profile
{
    public LeaveProfile()
    {
        CreateMap<Leave, LeaveDto>();

        CreateMap<CreateLeaveDto, Leave>();

        CreateMap<UpdateLeaveDto, Leave>();
    }
}