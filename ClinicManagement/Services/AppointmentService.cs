using ClinicManagement.DTOs.Appointment;
using ClinicManagement.Models;
using ClinicManagement.Repositories;

namespace ClinicManagement.Services;

public class AppointmentService
{
    private readonly AppointmentRepository _appointmentRepo;
    private readonly AppointmentSlotRepository _slotRepo;

    public AppointmentService(
        AppointmentRepository appointmentRepo,
        AppointmentSlotRepository slotRepo)
    {
        _appointmentRepo = appointmentRepo;
        _slotRepo = slotRepo;
    }

    public async Task<List<AppointmentResponseDto>> GetAllAsync()
    {
        var appointments = await _appointmentRepo.GetAllAsync();
        return appointments.Select(MapToDto).ToList();
    }

    public async Task<(bool Success, string Message, AppointmentResponseDto? Data)> GetByIdAsync(int id)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(id);
        if (appointment == null)
            return (false, "Không tìm thấy lịch hẹn.", null);

        return (true, "OK", MapToDto(appointment));
    }

    public async Task<List<AppointmentResponseDto>> GetByPatientIdAsync(int patientId)
    {
        var appointments = await _appointmentRepo.GetByPatientIdAsync(patientId);
        return appointments.Select(MapToDto).ToList();
    }

    public async Task<List<AppointmentResponseDto>> GetByDoctorIdAsync(int doctorId)
    {
        var appointments = await _appointmentRepo.GetByDoctorIdAsync(doctorId);
        return appointments.Select(MapToDto).ToList();
    }

    public async Task<(bool Success, string Message, AppointmentResponseDto? Data)> CreateAsync(AppointmentCreateDto dto)
    {
        var slot = await _slotRepo.GetByIdAsync(dto.SlotId);
        if (slot == null)
            return (false, "Không tìm thấy slot.", null);

        if (slot.Status != "Available")
            return (false, "Slot này đã được đặt hoặc không khả dụng.", null);

        if (slot.DoctorId != dto.DoctorId)
            return (false, "Slot không thuộc bác sĩ này.", null);

        var appointment = new Appointment
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            SlotId = dto.SlotId,
            Reason = dto.Reason?.Trim(),
            Status = "Pending"
        };

        await _appointmentRepo.CreateAsync(appointment);

        slot.Status = "Booked";
        await _slotRepo.UpdateAsync(slot);

        var created = await _appointmentRepo.GetByIdAsync(appointment.Id);
        return (true, "Đặt lịch hẹn thành công.", MapToDto(created!));
    }

    public async Task<(bool Success, string Message, AppointmentResponseDto? Data)> ConfirmAsync(int id)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(id);
        if (appointment == null)
            return (false, "Không tìm thấy lịch hẹn.", null);

        if (appointment.Status != "Pending")
            return (false, "Chỉ có thể xác nhận lịch hẹn đang chờ.", null);

        appointment.Status = "Confirmed";
        await _appointmentRepo.UpdateAsync(appointment);
        return (true, "Xác nhận lịch hẹn thành công.", MapToDto(appointment));
    }

    public async Task<(bool Success, string Message, AppointmentResponseDto? Data)> CancelAsync(int id, AppointmentCancelDto dto)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(id);
        if (appointment == null)
            return (false, "Không tìm thấy lịch hẹn.", null);

        if (appointment.Status == "Completed")
            return (false, "Không thể huỷ lịch hẹn đã hoàn thành.", null);

        if (appointment.Status == "Cancelled")
            return (false, "Lịch hẹn đã được huỷ trước đó.", null);

        appointment.Status = "Cancelled";
        appointment.CancelReason = dto.CancelReason.Trim();
        await _appointmentRepo.UpdateAsync(appointment);

        var slot = await _slotRepo.GetByIdAsync(appointment.SlotId);
        if (slot != null)
        {
            slot.Status = "Available";
            await _slotRepo.UpdateAsync(slot);
        }

        return (true, "Huỷ lịch hẹn thành công.", MapToDto(appointment));
    }

    public async Task<(bool Success, string Message, AppointmentResponseDto? Data)> CompleteAsync(int id)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(id);
        if (appointment == null)
            return (false, "Không tìm thấy lịch hẹn.", null);

        if (appointment.Status != "Confirmed")
            return (false, "Chỉ có thể hoàn thành lịch hẹn đã xác nhận.", null);

        // Kiểm tra ngày khám đã tới chưa
        var today = DateOnly.FromDateTime(DateTime.Today);
        if (appointment.AppointmentSlot.SlotDate > today)
            return (false, "Chưa tới ngày khám, không thể đánh dấu hoàn thành.", null);

        appointment.Status = "Completed";
        await _appointmentRepo.UpdateAsync(appointment);
        return (true, "Đánh dấu khám xong thành công.", MapToDto(appointment));
    }

    private static AppointmentResponseDto MapToDto(Appointment a) => new()
    {
        Id = a.Id,
        PatientId = a.PatientId,
        PatientName = a.Patient.User.FullName,
        DoctorId = a.DoctorId,
        DoctorName = a.Doctor.User.FullName,
        SpecialtyName = a.Doctor.Specialty.Name,
        SlotId = a.SlotId,
        SlotDate = a.AppointmentSlot.SlotDate,
        SlotTime = a.AppointmentSlot.SlotTime,
        Status = a.Status,
        Reason = a.Reason,
        CancelReason = a.CancelReason,
        Notes = a.Notes,
        CreatedAt = a.CreatedAt,
        HasReview = a.Review != null,
        HasPayment = a.Payment != null
    };

    public async Task<(bool, string)> DeleteAsync(int id)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(id);
        if (appointment == null)
            return (false, "Không tìm thấy lịch hẹn.");

        if (appointment.Status != "Cancelled" && appointment.Status != "Completed")
            return (false, "Chỉ có thể xóa lịch hẹn đã huỷ hoặc hoàn thành.");

        if (appointment.MedicalRecord != null)
            return (false, "Không thể xóa vì lịch hẹn đã có hồ sơ bệnh án.");

        if (appointment.Payment != null)
            return (false, "Không thể xóa vì lịch hẹn đã có hóa đơn.");

        if (appointment.Review != null)
            return (false, "Không thể xóa vì lịch hẹn đã có đánh giá.");

        await _appointmentRepo.DeleteAsync(appointment);
        return (true, "Xóa lịch hẹn thành công.");
    }
}