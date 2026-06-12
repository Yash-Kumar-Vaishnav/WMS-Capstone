using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.Attendance;

public class CheckOutDto
{
    [Required]
    public DateTime CheckOut { get; set; } = DateTime.Now;
}
