using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models;

public class Payment
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Appointment")]
    public int AppointmentId { get; set; }

    [Required, MaxLength(50)]
    public string InvoiceCode { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public string Status { get; set; } = "Unpaid";

    [Required]
    public string Method { get; set; } = "Cash";

    public string? Notes { get; set; }

    public DateTime? PaidAt { get; set; }

    public Appointment Appointment { get; set; } = null!;
}