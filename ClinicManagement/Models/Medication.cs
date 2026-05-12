using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models;

public class Medication
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Unit { get; set; }

    public string? Description { get; set; }

    [Range(0, 100000000)]
    public decimal Price { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}