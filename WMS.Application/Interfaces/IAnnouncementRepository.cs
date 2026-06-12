using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface IAnnouncementRepository
    : IGenericRepository<Announcement>
{
    Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync();
}