using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.Employee;

public class UpdateEmployeeDto : IValidatableObject
{
    [Required]
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
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number.")]
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

    public string Status { get; set; } = "Active";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DOB.Date > DateTime.Today)
            yield return new ValidationResult("Date of Birth cannot be a future date.", new[] { nameof(DOB) });

        if (DOJ.Date > DateTime.Today)
            yield return new ValidationResult("Date of Joining cannot be a future date.", new[] { nameof(DOJ) });
    }
}