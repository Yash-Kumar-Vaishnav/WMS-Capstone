using WMS.Application.DTOs.Announcement;

namespace WMS.Application.Interfaces;

public interface IAnnouncementService
{
    Task<IEnumerable<AnnouncementDto>> GetAllAsync();

    Task<IEnumerable<AnnouncementDto>> GetActiveAsync();

    Task<AnnouncementDto?> GetByIdAsync(int id);

    Task<int> CreateAsync(CreateAnnouncementDto dto);

    Task<bool> UpdateAsync(UpdateAnnouncementDto dto);

    Task<bool> DeleteAsync(int id);
}