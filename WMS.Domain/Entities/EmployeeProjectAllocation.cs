using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Domain.Entities;

public class EmployeeProjectAllocation
{
    [Key]
    public int AllocationId { get; set; }

    [Required]
    public int EmpId { get; set; }

    [Required]
    public int ProjectId { get; set; }

    [Required]
    public DateTime AssignedOn { get; set; }

    [Required]
    public DateTime CreateDate { get; set; }

    [Required]
    [StringLength(50)]
    public string CreatedBy { get; set; } = string.Empty;

    public bool Status { get; set; } = true;

    [StringLength(50)]
    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    [ForeignKey(nameof(EmpId))]
    public Employee? Employee { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project? Project { get; set; }
}