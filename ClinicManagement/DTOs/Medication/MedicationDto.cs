using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.DTOs.Medication;

public class MedicationCreateDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Unit { get; set; }

    public string? Description { get; set; }
}

public class MedicationUpdateDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Unit { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}

public class MedicationResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Unit { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}