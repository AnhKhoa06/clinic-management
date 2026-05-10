using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.DTOs.MedicalRecord;

public class MedicalRecordCreateDto
{
    [Required]
    public int AppointmentId { get; set; }

    public string? Symptoms { get; set; }

    public string? Diagnosis { get; set; }

    public string? TreatmentNotes { get; set; }

    public DateOnly? FollowUpDate { get; set; }

    public List<PrescriptionCreateDto> Prescriptions { get; set; } = new();
}

public class MedicalRecordUpdateDto
{
    public string? Symptoms { get; set; }

    public string? Diagnosis { get; set; }

    public string? TreatmentNotes { get; set; }

    public DateOnly? FollowUpDate { get; set; }
}

public class MedicalRecordResponseDto
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateOnly SlotDate { get; set; }
    public TimeOnly SlotTime { get; set; }
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public string? TreatmentNotes { get; set; }
    public DateOnly? FollowUpDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<PrescriptionResponseDto> Prescriptions { get; set; } = new();
}

public class PrescriptionCreateDto
{
    [Required]
    public int MedicationId { get; set; }

    [Required, MaxLength(100)]
    public string Dosage { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Frequency { get; set; } = string.Empty;

    [Required, Range(1, 365)]
    public int DurationDays { get; set; }

    public string? Notes { get; set; }
}

public class PrescriptionResponseDto
{
    public int Id { get; set; }
    public int MedicationId { get; set; }
    public string MedicationName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public string? Notes { get; set; }
}