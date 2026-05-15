using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.DTOs.MedicalRecord;

public class MedicalRecordCreateDto
{
    [Required]
    public int AppointmentId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập triệu chứng.")]
    public string? Symptoms { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập chẩn đoán.")]
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
    public bool HasPayment { get; set; }
    public List<PrescriptionResponseDto> Prescriptions { get; set; } = new();
}

public class PrescriptionCreateDto
{
    [Required]
    public int MedicationId { get; set; }

    [Required, Range(0.5, 100)]
    public decimal DosagePerTime { get; set; }   // Số viên/lần

    [Required, Range(1, 20)]
    public int TimesPerDay { get; set; }          // Số lần/ngày

    [Required, Range(1, 365)]
    public int DurationDays { get; set; }         // Số ngày

    [Required, Range(1, 10000)]
    public int Quantity { get; set; }             // Tự tính ở client, gửi lên để lưu

    public decimal UnitPrice { get; set; } = 0;

    public string? Notes { get; set; }
}

public class PrescriptionResponseDto
{
    public int Id { get; set; }
    public int MedicationId { get; set; }
    public string MedicationName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal DosagePerTime { get; set; }
    public int TimesPerDay { get; set; }
    public int DurationDays { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
    public string? Notes { get; set; }
}