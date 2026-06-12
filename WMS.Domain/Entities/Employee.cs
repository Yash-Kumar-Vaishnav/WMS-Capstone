using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Common;

namespace WMS.Domain.Entities;

public class Employee : BaseEntity
{
    [Key]
    public int EmployeeId { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(80)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(1)]
    public string Gender { get; set; } = string.Empty;

    [Required]
    public DateTime DOB { get; set; }

    [Required]
    public DateTime DOJ { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public int RoleId { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = "Active";

    // Navigation Properties

    [ForeignKey(nameof(DepartmentId))]
    public Department? Department { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Role? Role { get; set; }

    public ICollection<Attendance> Attendances { get; set; }
        = new List<Attendance>();

    public ICollection<Leave> Leaves { get; set; }
        = new List<Leave>();

    public ICollection<EmployeeProjectAllocation> EmployeeProjectAllocations { get; set; }
        = new List<EmployeeProjectAllocation>();
}