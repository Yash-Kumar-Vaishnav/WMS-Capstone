using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.Client;

public class UpdateClientDto
{
    [Required]
    public int ClientId { get; set; }

    [Required]
    [StringLength(100)]
    public string ClientName { get; set; } = string.Empty;

    [StringLength(255)]
    public string? ClientAddress { get; set; }

    [Range(1000000000, 999999999999999, ErrorMessage = "Invalid phone number length.")]
    public long? ClientPhoneNumber { get; set; }

    [StringLength(20)]
    public string? ClientLocation { get; set; }

    public bool Status { get; set; }
}