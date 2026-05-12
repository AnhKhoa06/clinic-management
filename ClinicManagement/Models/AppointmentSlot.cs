using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models;

public class AppointmentSlot
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("WorkingSchedule")]
    public int WorkingScheduleId { get; set; }

    [ForeignKey("Doctor")]
    public int DoctorId { get; set; }

    public DateOnly SlotDate { get; set; }

    public TimeOnly SlotTime { get; set; }

    [Required]
    public string Status { get; set; } = "Available";

    public WorkingSchedule WorkingSchedule { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
}