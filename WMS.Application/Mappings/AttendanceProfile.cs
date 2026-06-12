using AutoMapper;
using WMS.Application.DTOs.Attendance;
using WMS.Domain.Entities;

namespace WMS.Application.Mappings;

public class AttendanceProfile : Profile
{
    public AttendanceProfile()
    {
        CreateMap<Attendance, AttendanceDto>();

        CreateMap<CreateAttendanceDto, Attendance>();

        CreateMap<UpdateAttendanceDto, Attendance>();
    }
}