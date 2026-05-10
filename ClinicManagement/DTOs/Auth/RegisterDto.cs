using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.DTOs.Auth;

public class RegisterDto
{
    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }
}