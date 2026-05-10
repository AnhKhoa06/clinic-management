using ClinicManagement.DTOs.Patient;
using ClinicManagement.Models;
using ClinicManagement.Repositories;
using ClinicManagement.DTOs.Appointment;

namespace ClinicManagement.Services;

public class PatientService
{
    private readonly PatientRepository _patientRepo;
    private readonly AuthRepository _authRepo;
    private readonly AppointmentRepository _appointmentRepo;

    public PatientService(PatientRepository patientRepo, AuthRepository authRepo, AppointmentRepository appointmentRepo)
    {
        _patientRepo = patientRepo;
        _authRepo = authRepo;
        _appointmentRepo = appointmentRepo;
    }

    public async Task<List<AppointmentResponseDto>> GetAppointmentsByPatientIdAsync(int patientId)
    {
        var appointments = await _appointmentRepo.GetByPatientIdAsync(patientId);
        return appointments.Select(a => new AppointmentResponseDto
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
            CreatedAt = a.CreatedAt
        }).ToList();
    }

    public async Task<List<PatientResponseDto>> GetAllAsync()
    {
        var patients = await _patientRepo.GetAllAsync();
        return patients.Select(MapToDto).ToList();
    }

    public async Task<(bool Success, string Message, PatientResponseDto? Data)> GetByIdAsync(int id)
    {
        var patient = await _patientRepo.GetByIdAsync(id);
        if (patient == null)
            return (false, "Không tìm thấy bệnh nhân.", null);

        return (true, "OK", MapToDto(patient));
    }

    public async Task<(bool Success, string Message, PatientResponseDto? Data)> UpdateAsync(int id, PatientUpdateDto dto)
    {
        var patient = await _patientRepo.GetByIdAsync(id);
        if (patient == null)
            return (false, "Không tìm thấy bệnh nhân.", null);

        patient.Gender = dto.Gender?.Trim();
        patient.DateOfBirth = dto.DateOfBirth;
        patient.Address = dto.Address?.Trim();
        patient.BloodType = dto.BloodType?.Trim();
        patient.EmergencyContact = dto.EmergencyContact?.Trim();

        await _patientRepo.UpdateAsync(patient);
        return (true, "Cập nhật thành công.", MapToDto(patient));
    }

    public async Task<(bool Success, string Message, PatientResponseDto? Data)> GetByUserIdAsync(int userId)
    {
        var patient = await _patientRepo.GetByUserIdAsync(userId);
        if (patient == null)
        {
            var user = await _authRepo.GetUserByIdAsync(userId);
            if (user == null)
                return (false, "Không tìm thấy người dùng.", null);

            var newPatient = new Patient { UserId = userId };
            await _patientRepo.CreateAsync(newPatient);
            patient = await _patientRepo.GetByUserIdAsync(userId);
        }

        return (true, "OK", MapToDto(patient!));
    }

    private static PatientResponseDto MapToDto(Patient p) => new()
    {
        Id = p.Id,
        UserId = p.UserId,
        FullName = p.User.FullName,
        Email = p.User.Email,
        Phone = p.User.Phone,
        Gender = p.Gender,
        DateOfBirth = p.DateOfBirth,
        Address = p.Address,
        BloodType = p.BloodType,
        EmergencyContact = p.EmergencyContact
    };
}