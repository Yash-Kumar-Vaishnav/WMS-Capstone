using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.Leave;

public class CreateLeaveDto : IValidatableObject
{
    [Required]
    public int EmpId { get; set; }

    [Required]
    public string LeaveType { get; set; } = string.Empty;

    public string? Reason { get; set; }

    [Required]
    public DateTime FromDate { get; set; }

    [Required]
    public DateTime ToDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FromDate.Date < DateTime.Now.Date)
            yield return new ValidationResult("From Date cannot be in the past.", new[] { nameof(FromDate) });
        
        if (ToDate.Date < FromDate.Date)
            yield return new ValidationResult("To Date cannot be earlier than From Date.", new[] { nameof(ToDate) });
    }
}