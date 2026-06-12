using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities;

public class Client
{
    [Key]
    public int ClientId { get; set; }

    [Required]
    [StringLength(100)]
    public string ClientName { get; set; } = string.Empty;

    public string? ClientAddress { get; set; }

    public long? ClientPhoneNumber { get; set; }

    [StringLength(20)]
    public string? ClientLocation { get; set; }

    public bool Status { get; set; } = true;
}