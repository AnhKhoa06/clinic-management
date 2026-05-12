using ClinicManagement.DTOs.MedicalRecord;
using ClinicManagement.Models;
using ClinicManagement.Repositories;

namespace ClinicManagement.Services;

public class MedicalRecordService
{
    private readonly MedicalRecordRepository _medicalRecordRepo;
    private readonly AppointmentRepository _appointmentRepo;

    public MedicalRecordService(
        MedicalRecordRepository medicalRecordRepo,
        AppointmentRepository appointmentRepo)
    {
        _medicalRecordRepo = medicalRecordRepo;
        _appointmentRepo = appointmentRepo;
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
        var created = await _medicalRecordRepo.GetByIdAsync(record.Id);
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

    private static MedicalRecordResponseDto MapToDto(MedicalRecord mr) => new()
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
}