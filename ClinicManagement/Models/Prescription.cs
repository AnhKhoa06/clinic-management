using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models;

public class Prescription
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("MedicalRecord")]
    public int MedicalRecordId { get; set; }

    [ForeignKey("Medication")]
    public int MedicationId { get; set; }

    [MaxLength(100)]
    public string? Dosage { get; set; }

    [MaxLength(100)]
    public string? Frequency { get; set; }

    public int DurationDays { get; set; }

    public string? Notes { get; set; }

    public MedicalRecord MedicalRecord { get; set; } = null!;
    public Medication Medication { get; set; } = null!;
}