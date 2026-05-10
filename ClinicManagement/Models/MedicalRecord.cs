using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models;

public class MedicalRecord
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Appointment")]
    public int AppointmentId { get; set; }

    [ForeignKey("Patient")]
    public int PatientId { get; set; }

    [ForeignKey("Doctor")]
    public int DoctorId { get; set; }

    public string? Symptoms { get; set; }

    public string? Diagnosis { get; set; }

    public string? TreatmentNotes { get; set; }

    public DateOnly? FollowUpDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Appointment Appointment { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}