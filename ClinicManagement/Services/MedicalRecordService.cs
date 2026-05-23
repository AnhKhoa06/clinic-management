using ClinicManagement.DTOs.MedicalRecord;
using ClinicManagement.Models;
using ClinicManagement.Repositories;

namespace ClinicManagement.Services;

public class MedicalRecordService
{
    private readonly MedicalRecordRepository _medicalRecordRepo;
    private readonly AppointmentRepository _appointmentRepo;
    private readonly PaymentRepository _paymentRepo;
    private readonly NotificationService _notificationService;

    public MedicalRecordService(
        MedicalRecordRepository medicalRecordRepo,
        AppointmentRepository appointmentRepo,
        PaymentRepository paymentRepo,
        NotificationService notificationService)
    {
        _medicalRecordRepo = medicalRecordRepo;
        _appointmentRepo = appointmentRepo;
        _paymentRepo = paymentRepo;
        _notificationService = notificationService;
    }

    public async Task<List<MedicalRecordResponseDto>> GetAllAsync()
    {
        var records = await _medicalRecordRepo.GetAllAsync();
        return records.Select(MapToDto).ToList();
    }

    public async Task<(bool Success, string Message, MedicalRecordResponseDto? Data)> GetByIdAsync(int id)
    {
        var record = await _medicalRecordRepo.GetByIdAsync(id);
        if (record == null)
            return (false, "Không tìm thấy hồ sơ bệnh án.", null);

        return (true, "OK", MapToDto(record));
    }

    public async Task<List<MedicalRecordResponseDto>> GetByPatientIdAsync(int patientId)
    {
        var records = await _medicalRecordRepo.GetByPatientIdAsync(patientId);
        return records.Select(MapToDto).ToList();
    }

    public async Task<(bool Success, string Message, MedicalRecordResponseDto? Data)> CreateAsync(MedicalRecordCreateDto dto)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(dto.AppointmentId);
        if (appointment == null)
            return (false, "Không tìm thấy lịch hẹn.", null);

        if (appointment.Status != "Completed")
            return (false, "Chỉ tạo hồ sơ cho lịch hẹn đã hoàn thành.", null);

        if (await _medicalRecordRepo.ExistsByAppointmentIdAsync(dto.AppointmentId))
            return (false, "Lịch hẹn này đã có hồ sơ bệnh án.", null);

        var record = new MedicalRecord
        {
            AppointmentId = dto.AppointmentId,
            PatientId = appointment.PatientId,
            DoctorId = appointment.DoctorId,
            Symptoms = dto.Symptoms?.Trim(),
            Diagnosis = dto.Diagnosis?.Trim(),
            TreatmentNotes = dto.TreatmentNotes?.Trim(),
            FollowUpDate = dto.FollowUpDate,
            Prescriptions = dto.Prescriptions.Select(p => new Prescription
            {
                MedicationId = p.MedicationId,
                DosagePerTime = p.DosagePerTime,
                TimesPerDay = p.TimesPerDay,
                DurationDays = p.DurationDays,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice,
                Notes = p.Notes?.Trim()
            }).ToList()
        };

        await _medicalRecordRepo.CreateAsync(record);
        
        // Tự động tạo hóa đơn
        var created = await _medicalRecordRepo.GetByIdAsync(record.Id);
        var medicationFee = created!.Prescriptions.Sum(p => p.Quantity * p.UnitPrice);
        var examinationFee = created.Doctor.ExaminationFee;

        var payment = new Payment
        {
            AppointmentId = dto.AppointmentId,
            InvoiceCode = $"INV-{DateTime.Now:yyyyMMdd}-{dto.AppointmentId:D4}",
            ExaminationFee = examinationFee,
            MedicationFee = medicationFee,
            Amount = examinationFee + medicationFee,
            Status = "Unpaid",
            Method = "Cash", // default, Admin đổi sau
            Notes = null
        };
        await _paymentRepo.CreateAsync(payment);

        try
        {
            await _notificationService.CreateAsync(new Notification
            {
                UserId = created.Patient.UserId,
                Title = "Đã tạo hồ sơ bệnh án",
                Message = $"Bác sĩ {created.Doctor.User.FullName} đã tạo hồ sơ bệnh án cho lịch hẹn {created.Appointment.AppointmentSlot.SlotDate:dd/MM/yyyy} {created.Appointment.AppointmentSlot.SlotTime}.",
                Link = $"/MedicalRecord/MyDetail/{created.Id}"
            });

            await _notificationService.CreateAsync(new Notification
            {
                Role = "Admin",
                Title = "Đã tạo hồ sơ bệnh án",
                Message = $"Hồ sơ bệnh án #{created.Id} đã được tạo cho bệnh nhân {created.Patient.User.FullName}.",
                Link = $"/MedicalRecord/Detail/{created.Id}"
            });

            await _notificationService.CreateAsync(new Notification
            {
                Role = "Doctor",
                Title = "Đã tạo hóa đơn",
                Message = $"Hóa đơn cho lịch hẹn {created.Appointment.AppointmentSlot.SlotDate:dd/MM/yyyy} {created.Appointment.AppointmentSlot.SlotTime} đã được tạo.",
                Link = $"/Payment/Detail/{payment.Id}"
            });
        }
        catch { }

        return (true, "Tạo hồ sơ bệnh án thành công.", MapToDto(created!));
    }

    public async Task<(bool Success, string Message, MedicalRecordResponseDto? Data)> UpdateAsync(int id, MedicalRecordUpdateDto dto)
    {
        var record = await _medicalRecordRepo.GetByIdAsync(id);
        if (record == null)
            return (false, "Không tìm thấy hồ sơ bệnh án.", null);

        record.Symptoms = dto.Symptoms?.Trim();
        record.Diagnosis = dto.Diagnosis?.Trim();
        record.TreatmentNotes = dto.TreatmentNotes?.Trim();
        record.FollowUpDate = dto.FollowUpDate;

        await _medicalRecordRepo.UpdateAsync(record);
        return (true, "Cập nhật hồ sơ bệnh án thành công.", MapToDto(record));
    }

    public async Task<(bool, string)> DeleteAsync(int id)
    {
        var record = await _medicalRecordRepo.GetByIdAsync(id);
        if (record == null)
            return (false, "Không tìm thấy hồ sơ bệnh án.");

        await _medicalRecordRepo.DeleteAsync(record);
        return (true, "Xóa hồ sơ bệnh án thành công.");
    }

    public async Task<List<MedicalRecordResponseDto>> GetByDoctorIdAsync(int doctorId)
    {
        var records = await _medicalRecordRepo.GetByDoctorIdAsync(doctorId);
        return records.Select(MapToDto).ToList();
    }

    public async Task<MedicalRecordResponseDto?> GetByAppointmentIdAsync(int appointmentId)
    {
        var record = await _medicalRecordRepo.GetByAppointmentIdAsync(appointmentId);
        return record == null ? null : MapToDto(record);
    }

    private MedicalRecordResponseDto MapToDto(MedicalRecord mr) => new()
    {
        Id = mr.Id,
        AppointmentId = mr.AppointmentId,
        PatientId = mr.PatientId,
        PatientName = mr.Patient.User.FullName,
        DoctorId = mr.DoctorId,
        DoctorName = mr.Doctor.User.FullName,
        SlotDate = mr.Appointment.AppointmentSlot.SlotDate,
        SlotTime = mr.Appointment.AppointmentSlot.SlotTime,
        Symptoms = mr.Symptoms,
        Diagnosis = mr.Diagnosis,
        TreatmentNotes = mr.TreatmentNotes,
        FollowUpDate = mr.FollowUpDate,
        CreatedAt = mr.CreatedAt,
        HasPayment = _paymentRepo.ExistsByAppointmentIdAsync(mr.AppointmentId).Result,
        Prescriptions = mr.Prescriptions.Select(p => new PrescriptionResponseDto
        {
            Id = p.Id,
            MedicationId = p.MedicationId,
            MedicationName = p.Medication.Name,
            Unit = p.Medication.Unit ?? "",
            DosagePerTime = p.DosagePerTime,
            TimesPerDay = p.TimesPerDay,
            DurationDays = p.DurationDays,
            Quantity = p.Quantity,
            UnitPrice = p.UnitPrice,
            Notes = p.Notes
        }).ToList()
    };
}