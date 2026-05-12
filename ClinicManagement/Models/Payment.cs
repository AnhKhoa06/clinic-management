using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models;

public class Payment
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Appointment")]
    public int AppointmentId { get; set; }

    public string InvoiceCode { get; set; } = string.Empty;

    public decimal ExaminationFee { get; set; } = 0;
    public decimal MedicationFee { get; set; } = 0;
    public decimal Amount { get; set; } // = ExaminationFee + MedicationFee

    public string Status { get; set; } = "Unpaid";
    public string Method { get; set; } = "Cash";
    public string? Notes { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Appointment Appointment { get; set; } = null!;
}