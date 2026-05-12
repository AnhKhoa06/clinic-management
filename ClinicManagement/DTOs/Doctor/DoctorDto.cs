using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.DTOs.Doctor;

public class DoctorCreateDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int SpecialtyId { get; set; }

    [Required, MaxLength(50)]
    public string LicenseNumber { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public string? AvatarUrl { get; set; }
}

public class DoctorCreateWithAccountDto
{
    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Phone { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public int SpecialtyId { get; set; }

    [Required, MaxLength(50)]
    public string LicenseNumber { get; set; } = string.Empty;

    public string? Bio { get; set; }
}

public class DoctorUpdateDto
{
    [Required]
    public int SpecialtyId { get; set; }

    [Required, MaxLength(50)]
    public string LicenseNumber { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public string? AvatarUrl { get; set; }
}

public class DoctorResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int SpecialtyId { get; set; }
    public string SpecialtyName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; }
    public double AverageRating { get; set; }
    public decimal ExaminationFee { get; set; }
}