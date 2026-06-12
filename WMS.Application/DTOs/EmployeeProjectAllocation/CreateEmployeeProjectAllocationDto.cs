using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.EmployeeProjectAllocation;

public class CreateEmployeeProjectAllocationDto : IValidatableObject
{
    [Required]
    public int EmpId { get; set; }

    [Required]
    public int ProjectId { get; set; }

    [Required]
    public DateTime AssignedOn { get; set; }

    [Required]
    public string CreatedBy { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AssignedOn.Date < DateTime.Today)
        {
            yield return new ValidationResult("Project assignment date cannot be earlier than today.", new[] { nameof(AssignedOn) });
        }
    }
}