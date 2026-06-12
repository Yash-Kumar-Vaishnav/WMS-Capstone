using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.Attendance;

public class CheckInDto
{
    [Required]
    public int EmpId { get; set; }

    [Required]
    public DateTime CheckIn { get; set; } = DateTime.Now;

    [StringLength(20)]
    public string? WorkMode { get; set; } = "WFO";

    public DateTime AttendanceDate { get; set; } = DateTime.Today;
}
