using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.Attendance;

public class UpdateAttendanceDto
{
    [Required]
    public int AttendanceId { get; set; }

    [Required]
    public int EmpId { get; set; }

    [Required]
    public DateTime CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }

    public double? TotalHours { get; set; }

    public string? WorkMode { get; set; }

    [Required]
    public DateTime AttendanceDate { get; set; }
}