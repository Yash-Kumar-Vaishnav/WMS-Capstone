using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface IProjectRepository
    : IGenericRepository<Project>
{
    Task<IEnumerable<Project>> GetActiveProjectsAsync();
}