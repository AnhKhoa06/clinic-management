using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models;

public class Appointment
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Patient")]
    public int PatientId { get; set; }

    [ForeignKey("Doctor")]
    public int DoctorId { get; set; }

    [ForeignKey("AppointmentSlot")]
    public int SlotId { get; set; }

    public string? Reason { get; set; }

    [Required]
    public string Status { get; set; } = "Pending";

    public string? CancelReason { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    public AppointmentSlot AppointmentSlot { get; set; } = null!;
    public MedicalRecord? MedicalRecord { get; set; }
    public Payment? Payment { get; set; }
    public Review? Review { get; set; }
}