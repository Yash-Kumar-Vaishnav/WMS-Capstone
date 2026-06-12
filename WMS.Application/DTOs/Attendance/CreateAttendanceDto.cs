using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.Attendance;

public class CreateAttendanceDto
{
    [Required]
    public int EmpId { get; set; }

    [Required]
    public DateTime CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }

    public string? WorkMode { get; set; }

    [Required]
    public DateTime AttendanceDate { get; set; }
}