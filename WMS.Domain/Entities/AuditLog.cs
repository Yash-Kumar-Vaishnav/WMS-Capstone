using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities;

public class AuditLog
{
    [Key]
    public int AuditId { get; set; }

    [Required]
    public string EntityName { get; set; } = string.Empty;

    public int RecordId { get; set; }

    [Required]
    [StringLength(20)]
    public string Action { get; set; } = string.Empty;

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.Now;
}