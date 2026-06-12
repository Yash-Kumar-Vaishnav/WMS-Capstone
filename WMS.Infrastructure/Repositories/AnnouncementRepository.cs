using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public class AnnouncementRepository
    : GenericRepository<Announcement>,
      IAnnouncementRepository
{
    public AnnouncementRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync()
    {
        return await _context.Announcements
            .Where(x => x.IsActive)
            .ToListAsync();
    }
}