using System.ComponentModel.DataAnnotations;
using WMS.Domain.Common;

namespace WMS.Domain.Entities;

public class Department : BaseEntity
{
    public int DepartmentId { get; set; }

    [Required]
    [StringLength(100)]
    public string DepartmentName { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Description { get; set; }

    public ICollection<Employee> Employees { get; set; }
        = new List<Employee>();
}