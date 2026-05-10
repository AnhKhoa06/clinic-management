using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models;

public class Review
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Appointment")]
    public int AppointmentId { get; set; }

    [ForeignKey("Patient")]
    public int PatientId { get; set; }

    [ForeignKey("Doctor")]
    public int DoctorId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Appointment Appointment { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
}