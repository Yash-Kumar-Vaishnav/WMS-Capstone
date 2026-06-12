using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.EmployeeProjectAllocation;

public class UpdateEmployeeProjectAllocationDto : IValidatableObject
{
    [Required]
    public int AllocationId { get; set; }

    [Required]
    public int EmpId { get; set; }

    [Required]
    public int ProjectId { get; set; }

    [Required]
    public DateTime AssignedOn { get; set; }

    public bool Status { get; set; }

    public string? UpdatedBy { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AssignedOn.Date < DateTime.Today)
        {
            yield return new ValidationResult("Project assignment date cannot be earlier than today.", new[] { nameof(AssignedOn) });
        }
    }
}