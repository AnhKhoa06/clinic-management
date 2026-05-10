using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.DTOs.Appointment;

public class WorkingScheduleCreateDto
{
    [Required]
    public int DoctorId { get; set; }

    [Required, Range(0, 6)]
    public int DayOfWeek { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }

    [Required, Range(15, 60)]
    public int SlotDurationMinutes { get; set; } = 30;

    [Required, Range(1, 50)]
    public int MaxSlots { get; set; }
}

public class WorkingScheduleUpdateDto
{
    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }

    [Required, Range(15, 60)]
    public int SlotDurationMinutes { get; set; }

    [Required, Range(1, 50)]
    public int MaxSlots { get; set; }

    public bool IsActive { get; set; } = true;
}

public class WorkingScheduleResponseDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public int DayOfWeek { get; set; }
    public string DayOfWeekName { get; set; } = string.Empty;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int SlotDurationMinutes { get; set; }
    public int MaxSlots { get; set; }
    public bool IsActive { get; set; }
}