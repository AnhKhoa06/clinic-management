using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models;

public class WorkingSchedule
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Doctor")]
    public int DoctorId { get; set; }

    public int DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int SlotDurationMinutes { get; set; } = 30;

    public int MaxSlots { get; set; }

    public bool IsActive { get; set; } = true;

    public Doctor Doctor { get; set; } = null!;
    public ICollection<AppointmentSlot> AppointmentSlots { get; set; } = new List<AppointmentSlot>();
}