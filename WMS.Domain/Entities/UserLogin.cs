using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Domain.Entities;

public class UserLogin
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public int RoleId { get; set; }

    public DateTime? LastLogin { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Role? Role { get; set; }
}