using AutoMapper;
using WMS.Application.DTOs.Project;
using WMS.Domain.Entities;

namespace WMS.Application.Mappings;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectDto>();

        CreateMap<CreateProjectDto, Project>();

        CreateMap<UpdateProjectDto, Project>();
    }
}