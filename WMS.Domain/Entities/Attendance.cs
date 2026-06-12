using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Domain.Entities;

public class Attendance
{
    [Key]
    public int AttendanceId { get; set; }

    [Required]
    public int EmpId { get; set; }

    [Required]
    public DateTime CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }

    public double? TotalHours { get; set; }

    [StringLength(20)]
    public string? WorkMode { get; set; }

    [Required]
    public DateTime AttendanceDate { get; set; }

    [ForeignKey(nameof(EmpId))]
    public Employee? Employee { get; set; }
}