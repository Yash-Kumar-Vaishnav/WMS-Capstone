using AutoMapper;
using WMS.Application.DTOs.Announcement;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;

namespace WMS.Application.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly IMapper _mapper;

    public AnnouncementService(
        IAnnouncementRepository announcementRepository,
        IMapper mapper)
    {
        _announcementRepository = announcementRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AnnouncementDto>> GetAllAsync()
    {
        var announcements = await _announcementRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<AnnouncementDto>>(announcements);
    }

    public async Task<IEnumerable<AnnouncementDto>> GetActiveAsync()
    {
        var announcements = await _announcementRepository.GetActiveAnnouncementsAsync();

        return _mapper.Map<IEnumerable<AnnouncementDto>>(announcements);
    }

    public async Task<AnnouncementDto?> GetByIdAsync(int id)
    {
        var announcement = await _announcementRepository.GetByIdAsync(id);

        if (announcement == null)
            return null;

        return _mapper.Map<AnnouncementDto>(announcement);
    }

    public async Task<int> CreateAsync(CreateAnnouncementDto dto)
    {
        var announcement = _mapper.Map<Announcement>(dto);

        await _announcementRepository.AddAsync(announcement);
        await _announcementRepository.SaveChangesAsync();

        return announcement.AnnouncementId;
    }

    public async Task<bool> UpdateAsync(UpdateAnnouncementDto dto)
    {
        var announcement = await _announcementRepository
            .GetByIdAsync(dto.AnnouncementId);

        if (announcement == null)
            return false;

        _mapper.Map(dto, announcement);

        _announcementRepository.Update(announcement);
        await _announcementRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var announcement = await _announcementRepository.GetByIdAsync(id);

        if (announcement == null)
            return false;

        _announcementRepository.Delete(announcement);
        await _announcementRepository.SaveChangesAsync();

        return true;
    }
}