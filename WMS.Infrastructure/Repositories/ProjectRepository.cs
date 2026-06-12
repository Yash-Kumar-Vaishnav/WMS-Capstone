using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class ProjectRepository
    : GenericRepository<Project>,
      IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
    {
        return await _context.Projects
            .Where(x => x.Status == "Active")
            .ToListAsync();
    }
}