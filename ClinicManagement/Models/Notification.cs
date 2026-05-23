using System;

namespace ClinicManagement.Models;

public class Notification
{
    public int Id { get; set; }

    // If set, notification is for a specific user. Otherwise use Role to target groups (Admin/Doctor/Patient)
    public int? UserId { get; set; }

    public string? Role { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Optional link to navigate when clicking the notification
    public string? Link { get; set; }

    // navigation
    public User? User { get; set; }
}
