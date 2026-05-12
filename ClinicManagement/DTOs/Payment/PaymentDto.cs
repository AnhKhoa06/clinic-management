using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.DTOs.Payment;

public class PaymentCreateDto
{
    [Required]
    public int AppointmentId { get; set; }

    [Required]
    public string Method { get; set; } = "Cash";

    public string? Notes { get; set; }
}

public class PaymentResponseDto
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public string InvoiceCode { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public DateOnly SlotDate { get; set; }
    public decimal ExaminationFee { get; set; }
    public decimal MedicationFee { get; set; }
    public decimal Amount { get; set; } // = ExaminationFee + MedicationFee
    public string Status { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; }
}