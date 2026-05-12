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

    public decimal DosagePerTime { get; set; }  // Số viên/lần
    public int TimesPerDay { get; set; }         // Số lần/ngày
    public int DurationDays { get; set; }        // Số ngày

    public int Quantity { get; set; } = 0;       // Tự tính = DosagePerTime * TimesPerDay * DurationDays

    public decimal UnitPrice { get; set; } = 0;

    [NotMapped]
    public decimal TotalPrice => Quantity * UnitPrice;

    public string? Notes { get; set; }

    public MedicalRecord MedicalRecord { get; set; } = null!;
    public Medication Medication { get; set; } = null!;
}