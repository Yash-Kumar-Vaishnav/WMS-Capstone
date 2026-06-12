using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.Project;

public class CreateProjectDto : IValidatableObject
{
    [Required]
    [StringLength(100)]
    public string ProjectName { get; set; } = string.Empty;

    public int? ClientId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate.HasValue && StartDate.Value.Date < DateTime.Today)
        {
            yield return new ValidationResult("Project Start Date cannot be earlier than today.", new[] { nameof(StartDate) });
        }

        if (StartDate.HasValue && EndDate.HasValue && EndDate.Value.Date < StartDate.Value.Date)
        {
            yield return new ValidationResult("End Date cannot be earlier than Start Date.", new[] { nameof(EndDate) });
        }
    }
}