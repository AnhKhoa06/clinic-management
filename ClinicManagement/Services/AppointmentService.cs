using ClinicManagement.DTOs.Appointment;
using ClinicManagement.Models;
using ClinicManagement.Repositories;

namespace ClinicManagement.Services;

public class AppointmentService
{
    private readonly AppointmentRepository _appointmentRepo;
    private readonly AppointmentSlotRepository _slotRepo;
    private readonly NotificationService _notificationService;

    public AppointmentService(
        AppointmentRepository appointmentRepo,
        AppointmentSlotRepository slotRepo,
        NotificationService notificationService)
    {
        _appointmentRepo = appointmentRepo;
        _slotRepo = slotRepo;
        _notificationService = notificationService;
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
        // Send notifications: to doctor and admin
        try
        {
            if (created != null)
            {
                var doctorName = created.Doctor?.User?.FullName ?? "bác sĩ";
                var patientName = created.Patient?.User?.FullName ?? "bệnh nhân";
                var slotDateText = created.AppointmentSlot?.SlotDate.ToString("dd/MM/yyyy") ?? "ngày khám";
                var slotTimeText = created.AppointmentSlot?.SlotTime.ToString() ?? "";

                var patientNotif = new Notification
                {
                    UserId = created.Patient?.UserId,
                    Title = "Đặt lịch thành công",
                    Message = $"Bạn đã đặt lịch với bác sĩ {doctorName} vào {slotDateText} {slotTimeText}.",
                    Link = "/Appointment/MyAppointments"
                };
                await _notificationService.CreateAsync(patientNotif);

                var notifToDoctor = new ClinicManagement.Models.Notification
                {
                    UserId = created.Doctor?.UserId,
                    Title = "Lịch hẹn mới",
                    Message = $"Bệnh nhân {patientName} đã đặt lịch vào {slotDateText} {slotTimeText}",
                    Link = $"/Appointment/Detail/{created.Id}"
                };
                await _notificationService.CreateAsync(notifToDoctor);

                var notifToAdmin = new ClinicManagement.Models.Notification
                {
                    Role = "Admin",
                    Title = "Lịch hẹn mới",
                    Message = $"Lịch hẹn #{created.Id} được tạo bởi {patientName}",
                    Link = $"/Appointment/Detail/{created.Id}"
                };
                await _notificationService.CreateAsync(notifToAdmin);
            }
        }
        catch { /* non-blocking */ }

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
        // notify patient
        try
        {
            var doctorName = appointment.Doctor?.User?.FullName ?? "bác sĩ";
            var patientName = appointment.Patient?.User?.FullName ?? "bệnh nhân";
            var slotDateText = appointment.AppointmentSlot?.SlotDate.ToString("dd/MM/yyyy") ?? "ngày khám";
            var slotTimeText = appointment.AppointmentSlot?.SlotTime.ToString() ?? "";

            var doctorNotif = new Notification
            {
                UserId = appointment.Doctor?.UserId,
                Title = "Lịch hẹn đã được xác nhận",
                Message = $"Lịch hẹn với bệnh nhân {patientName} vào {slotDateText} {slotTimeText} đã được xác nhận.",
                Link = $"/Appointment/Detail/{appointment.Id}"
            };
            await _notificationService.CreateAsync(doctorNotif);

            var notif = new Notification
            {
                UserId = appointment.Patient?.UserId,
                Title = "Lịch hẹn đã được xác nhận",
                Message = $"Lịch hẹn với {doctorName} vào {slotDateText} {slotTimeText} đã được xác nhận.",
                Link = $"/Appointment/MyAppointments"
            };
            await _notificationService.CreateAsync(notif);

            await _notificationService.CreateAsync(new Notification
            {
                Role = "Admin",
                Title = "Lịch hẹn đã được xác nhận",
                Message = $"Lịch hẹn #{appointment.Id} đã được xác nhận.",
                Link = $"/Appointment/Detail/{appointment.Id}"
            });
        }
        catch { }

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

        // notify patient and doctor
        try
        {
            var doctorName = appointment.Doctor?.User?.FullName ?? "bác sĩ";
            var patientName = appointment.Patient?.User?.FullName ?? "bệnh nhân";
            var slotDateText = appointment.AppointmentSlot?.SlotDate.ToString("dd/MM/yyyy") ?? "ngày khám";
            var slotTimeText = appointment.AppointmentSlot?.SlotTime.ToString() ?? "";

            var notifPatient = new Notification
            {
                UserId = appointment.Patient?.UserId,
                Title = "Lịch hẹn đã bị huỷ",
                Message = $"Lịch hẹn với {doctorName} vào {slotDateText} {slotTimeText} đã bị huỷ. Lý do: {appointment.CancelReason}",
                Link = $"/Appointment/MyAppointments"
            };
            await _notificationService.CreateAsync(notifPatient);

            var notifDoctor = new Notification
            {
                UserId = appointment.Doctor?.UserId,
                Title = "Lịch hẹn đã bị huỷ",
                Message = $"Lịch hẹn của bệnh nhân {patientName} vào {slotDateText} {slotTimeText} đã bị huỷ. Lý do: {appointment.CancelReason}",
                Link = $"/Appointment/Detail/{appointment.Id}"
            };
            await _notificationService.CreateAsync(notifDoctor);

            await _notificationService.CreateAsync(new Notification
            {
                Role = "Admin",
                Title = "Lịch hẹn đã bị huỷ",
                Message = $"Lịch hẹn #{appointment.Id} đã bị huỷ.",
                Link = $"/Appointment/Detail/{appointment.Id}"
            });
        }
        catch { }

        return (true, "Huỷ lịch hẹn thành công.", MapToDto(appointment));
    }

    public async Task<(bool Success, string Message, AppointmentResponseDto? Data)> CompleteAsync(int id)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(id);
        if (appointment == null)
            return (false, "Không tìm thấy lịch hẹn.", null);

        if (appointment.Status != "Confirmed")
            return (false, "Chỉ có thể hoàn thành lịch hẹn đã xác nhận.", null);

        if (appointment.AppointmentSlot == null)
            return (false, "Không tìm thấy slot của lịch hẹn.", null);

        // Kiểm tra ngày khám đã tới chưa
        var today = DateOnly.FromDateTime(DateTime.Today);
        if (appointment.AppointmentSlot.SlotDate > today)
            return (false, "Chưa tới ngày khám, không thể đánh dấu hoàn thành.", null);

        appointment.Status = "Completed";
        await _appointmentRepo.UpdateAsync(appointment);
        try
        {
            var doctorName = appointment.Doctor?.User?.FullName ?? "bác sĩ";
            var slotDateText = appointment.AppointmentSlot?.SlotDate.ToString("dd/MM/yyyy") ?? "ngày khám";
            var slotTimeText = appointment.AppointmentSlot?.SlotTime.ToString() ?? "";
            var notif = new Notification
            {
                UserId = appointment.Patient?.UserId,
                Title = "Hoàn thành khám",
                Message = $"Lịch hẹn với {doctorName} vào {slotDateText} {slotTimeText} đã được đánh dấu hoàn thành.",
                Link = $"/Appointment/MyAppointments"
            };
            await _notificationService.CreateAsync(notif);

            await _notificationService.CreateAsync(new Notification
            {
                UserId = appointment.Doctor?.UserId,
                Title = "Đã hoàn thành khám",
                Message = $"Bạn đã hoàn thành khám cho bệnh nhân {appointment.Patient?.User?.FullName ?? "bệnh nhân"}.",
                Link = $"/Appointment/Detail/{appointment.Id}"
            });

            await _notificationService.CreateAsync(new Notification
            {
                Role = "Admin",
                Title = "Hoàn thành khám",
                Message = $"Lịch hẹn #{appointment.Id} đã được đánh dấu hoàn thành.",
                Link = $"/Appointment/Detail/{appointment.Id}"
            });
        }
        catch { }

        return (true, "Đánh dấu khám xong thành công.", MapToDto(appointment));
    }

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

    //admin: lấy toàn bộ lịch hẹn, đếm có bn cái đang Pending, trả về con số đó.
    public async Task<int> GetPendingCountAsync()
    {
        var all = await _appointmentRepo.GetAllAsync();
        return all.Count(a => a.Status == "Pending");
    }

    //doctor: chỉ lấy lịch hẹn của đúng bác sĩ đó, đếm có bn cái đang Pending, trả về con số đó.
    public async Task<int> GetPendingCountByDoctorAsync(int doctorId)
    {
        var all = await _appointmentRepo.GetByDoctorIdAsync(doctorId);
        return all.Count(a => a.Status == "Pending");
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
}