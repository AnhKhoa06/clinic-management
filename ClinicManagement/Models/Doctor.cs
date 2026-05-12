using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models;

public class Doctor
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Specialty")]
    public int SpecialtyId { get; set; }

    [Required, MaxLength(50)]
    public string LicenseNumber { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public string? AvatarUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public User User { get; set; } = null!;
    public Specialty Specialty { get; set; } = null!;
    public ICollection<WorkingSchedule> WorkingSchedules { get; set; } = new List<WorkingSchedule>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public decimal ExaminationFee { get; set; } = 0;
}