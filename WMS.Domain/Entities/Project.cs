using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Domain.Entities;

public class Project
{
    [Key]
    public int ProjectId { get; set; }

    [Required]
    [StringLength(100)]
    public string ProjectName { get; set; } = string.Empty;

    public int? ClientId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = "Active";

    [ForeignKey(nameof(ClientId))]
    public Client? Client { get; set; }

    public ICollection<EmployeeProjectAllocation> EmployeeProjectAllocations { get; set; }
        = new List<EmployeeProjectAllocation>();
}