using WMS.Application.DTOs.Project;

namespace WMS.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync();

    Task<ProjectDto?> GetByIdAsync(int id);

    Task<int> CreateAsync(CreateProjectDto dto);

    Task<bool> UpdateAsync(UpdateProjectDto dto);

    Task<bool> DeleteAsync(int id);
}