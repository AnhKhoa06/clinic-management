using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.DTOs.Review;

public class ReviewCreateDto
{
    [Required]
    public int AppointmentId { get; set; }

    [Required]
    public int DoctorId { get; set; }

    [Required, Range(1, 5)]
    public int Rating { get; set; }

    [MaxLength(500)]
    public string? Comment { get; set; }
}

public class ReviewResponseDto
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}