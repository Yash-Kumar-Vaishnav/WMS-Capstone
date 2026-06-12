using AutoMapper;
using WMS.Application.DTOs.Project;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;

namespace WMS.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public ProjectService(
        IProjectRepository projectRepository,
        IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        var projects = await _projectRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        if (project == null)
            return null;

        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<int> CreateAsync(CreateProjectDto dto)
    {
        if (dto.StartDate.HasValue && dto.StartDate.Value.Date < DateTime.Today)
            throw new ArgumentException("Project Start Date cannot be earlier than today.");

        if (dto.StartDate.HasValue && dto.EndDate.HasValue && dto.EndDate.Value.Date < dto.StartDate.Value.Date)
            throw new ArgumentException("End Date cannot be earlier than Start Date.");

        var project = _mapper.Map<Project>(dto);

        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();

        return project.ProjectId;
    }

    public async Task<bool> UpdateAsync(UpdateProjectDto dto)
    {
        if (dto.StartDate.HasValue && dto.StartDate.Value.Date < DateTime.Today)
            throw new ArgumentException("Project Start Date cannot be earlier than today.");

        if (dto.StartDate.HasValue && dto.EndDate.HasValue && dto.EndDate.Value.Date < dto.StartDate.Value.Date)
            throw new ArgumentException("End Date cannot be earlier than Start Date.");

        var project = await _projectRepository.GetByIdAsync(dto.ProjectId);

        if (project == null)
            return false;

        _mapper.Map(dto, project);

        _projectRepository.Update(project);
        await _projectRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        if (project == null)
            return false;

        _projectRepository.Delete(project);
        await _projectRepository.SaveChangesAsync();

        return true;
    }
}