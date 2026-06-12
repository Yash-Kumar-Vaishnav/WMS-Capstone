using AutoMapper;
using WMS.Application.DTOs.Announcement;
using WMS.Domain.Entities;

namespace WMS.Application.Mappings;

public class AnnouncementProfile : Profile
{
    public AnnouncementProfile()
    {
        CreateMap<Announcement, AnnouncementDto>();

        CreateMap<CreateAnnouncementDto, Announcement>();

        CreateMap<UpdateAnnouncementDto, Announcement>();
    }
}