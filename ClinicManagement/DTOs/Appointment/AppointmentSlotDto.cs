namespace ClinicManagement.DTOs.Appointment;

public class GenerateSlotsDto
{
    public int DoctorId { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
}

public class AppointmentSlotResponseDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateOnly SlotDate { get; set; }
    public TimeOnly SlotTime { get; set; }
    public string Status { get; set; } = string.Empty;
}