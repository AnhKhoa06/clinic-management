using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.DTOs.Patient;

public class PatientUpdateDto
{
    [MaxLength(10)]
    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    [MaxLength(10)]
    public string? BloodType { get; set; }

    [MaxLength(200)]
    public string? EmergencyContact { get; set; }
}

public class PatientResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? BloodType { get; set; }
    public string? EmergencyContact { get; set; }
}