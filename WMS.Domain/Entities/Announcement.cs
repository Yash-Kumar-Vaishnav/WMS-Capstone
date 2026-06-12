using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities;

public class Announcement
{
    [Key]
    public int AnnouncementId { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    [Required]
    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;
}