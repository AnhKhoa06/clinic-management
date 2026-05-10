using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models;

public class Specialty
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}