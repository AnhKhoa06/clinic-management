using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.DTOs.Appointment;

public class AppointmentCreateDto
{
    [Required]
    public int PatientId { get; set; }

    [Required]
    public int DoctorId { get; set; }

    [Required]
    public int SlotId { get; set; }

    public string? Reason { get; set; }
}

public class AppointmentCancelDto
{
    [Required]
    public string CancelReason { get; set; } = string.Empty;
}

public class AppointmentResponseDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string SpecialtyName { get; set; } = string.Empty;
    public int SlotId { get; set; }
    public DateOnly SlotDate { get; set; }
    public TimeOnly SlotTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public string? CancelReason { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool HasReview { get; set; }
    public bool HasPayment { get; set; }
}