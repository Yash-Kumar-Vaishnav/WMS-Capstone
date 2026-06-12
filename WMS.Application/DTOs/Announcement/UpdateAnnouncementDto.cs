using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.Announcement;

public class UpdateAnnouncementDto
{
    [Required]
    public int AnnouncementId { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}