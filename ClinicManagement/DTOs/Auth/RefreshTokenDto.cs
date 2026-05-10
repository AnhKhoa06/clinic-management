using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.DTOs.Auth;

public class RefreshTokenDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}