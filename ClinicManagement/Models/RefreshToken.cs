using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [Required]
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}