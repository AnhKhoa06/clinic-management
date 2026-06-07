using ClinicManagement.DTOs.Doctor;
using ClinicManagement.Models;
using ClinicManagement.Helpers;
using ClinicManagement.Repositories;

namespace ClinicManagement.Services;

public class DoctorService
{
    private readonly DoctorRepository _doctorRepo;
    private readonly AuthRepository _authRepo;

    public DoctorService(DoctorRepository doctorRepo, AuthRepository authRepo)
    {
        _doctorRepo = doctorRepo;
        _authRepo = authRepo;
    }

    public async Task<List<DoctorResponseDto>> GetAllAsync()
    {
        var doctors = await _doctorRepo.GetAllAsync();
        return doctors.Select(MapToDto).ToList();
    }

    public async Task<(bool Success, string Message, DoctorResponseDto? Data)> GetByIdAsync(int id)
    {
        var doctor = await _doctorRepo.GetByIdAsync(id);
        if (doctor == null)
            return (false, "Không tìm thấy bác sĩ.", null);

        return (true, "OK", MapToDto(doctor));
    }

    public async Task<List<DoctorResponseDto>> GetBySpecialtyAsync(int specialtyId)
    {
        var doctors = await _doctorRepo.GetBySpecialtyAsync(specialtyId);
        return doctors.Select(MapToDto).ToList();
    }

    public async Task<(bool Success, string Message, DoctorResponseDto? Data)> CreateAsync(DoctorCreateDto dto)
    {
        var user = await _authRepo.GetUserByIdAsync(dto.UserId);
        if (user == null)
            return (false, "Không tìm thấy người dùng.", null);

        if (await _doctorRepo.UserIdExistsAsync(dto.UserId))
            return (false, "Người dùng này đã là bác sĩ.", null);

        if (await _doctorRepo.LicenseExistsAsync(dto.LicenseNumber))
            return (false, "Số giấy phép hành nghề đã tồn tại.", null);

        user.Role = "Doctor";
        await _authRepo.UpdateUserAsync(user);

        var doctor = new Doctor
        {
            UserId = dto.UserId,
            SpecialtyId = dto.SpecialtyId,
            LicenseNumber = dto.LicenseNumber.Trim(),
            Bio = dto.Bio?.Trim(),
            AvatarUrl = dto.AvatarUrl,
            IsActive = true
        };

        await _doctorRepo.CreateAsync(doctor);
        var created = await _doctorRepo.GetByIdAsync(doctor.Id);
        return (true, "Tạo bác sĩ thành công.", MapToDto(created!));
    }

    public async Task<(bool Success, string Message, DoctorResponseDto? Data)> CreateWithAccountAsync(DoctorCreateWithAccountDto dto)
    {
        if (await _authRepo.EmailExistsAsync(dto.Email))
            return (false, "Email đã được sử dụng.", null);

        if (await _doctorRepo.LicenseExistsAsync(dto.LicenseNumber))
            return (false, "Số giấy phép hành nghề đã tồn tại.", null);

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.ToLower().Trim(),
            PasswordHash = PasswordHelper.HashPassword(dto.Password),
            Phone = dto.Phone.Trim(),
            Role = "Doctor",
            IsActive = true
        };

        await _authRepo.CreateUserAsync(user);

        var doctor = new Doctor
        {
            UserId = user.Id,
            SpecialtyId = dto.SpecialtyId,
            LicenseNumber = dto.LicenseNumber.Trim(),
            Bio = dto.Bio?.Trim(),
            IsActive = true
        };

        await _doctorRepo.CreateAsync(doctor);
        var created = await _doctorRepo.GetByIdAsync(doctor.Id);
        return (true, "Tạo bác sĩ thành công.", MapToDto(created!));
    }

    public async Task<(bool Success, string Message, DoctorResponseDto? Data)> UpdateAsync(int id, DoctorUpdateDto dto)
    {
        var doctor = await _doctorRepo.GetByIdAsync(id);
        if (doctor == null)
            return (false, "Không tìm thấy bác sĩ.", null);

        if (await _doctorRepo.LicenseExistsAsync(dto.LicenseNumber, excludeId: id))
            return (false, "Số giấy phép hành nghề đã tồn tại.", null);

        doctor.SpecialtyId = dto.SpecialtyId;
        doctor.LicenseNumber = dto.LicenseNumber.Trim();
        doctor.Bio = dto.Bio?.Trim();
        doctor.AvatarUrl = dto.AvatarUrl;

        await _doctorRepo.UpdateAsync(doctor);
        return (true, "Cập nhật bác sĩ thành công.", MapToDto(doctor));
    }

    public async Task<(bool Success, string Message)> ToggleActiveAsync(int id)
    {
        var doctor = await _doctorRepo.GetByIdAsync(id);
        if (doctor == null)
            return (false, "Không tìm thấy bác sĩ.");

        doctor.IsActive = !doctor.IsActive;
        await _doctorRepo.UpdateAsync(doctor);

        var status = doctor.IsActive ? "kích hoạt" : "vô hiệu hoá";
        return (true, $"Đã {status} bác sĩ thành công.");
    }

    public async Task<DoctorResponseDto?> GetByUserIdAsync(int userId)
    {
        var doctor = await _doctorRepo.GetByUserIdAsync(userId);
        if (doctor == null) return null;
        return MapToDto(doctor);
    }

    private static DoctorResponseDto MapToDto(Doctor d) => new()
    {
        Id = d.Id,
        UserId = d.UserId,
        FullName = d.User.FullName,
        Email = d.User.Email,
        Phone = d.User.Phone,
        SpecialtyId = d.SpecialtyId,
        SpecialtyName = d.Specialty.Name,
        LicenseNumber = d.LicenseNumber,
        Bio = d.Bio,
        AvatarUrl = d.AvatarUrl,
        IsActive = d.IsActive,
        ExaminationFee = d.ExaminationFee,
        AverageRating = d.Reviews.Any()
            ? Math.Round(d.Reviews.Average(r => r.Rating), 1)
            : 0
    };

    public async Task<(bool, string)> DeleteAsync(int id)
    {
        var doctor = await _doctorRepo.GetByIdAsync(id);
        if (doctor == null)
            return (false, "Không tìm thấy bác sĩ.");

        if (doctor.Appointments.Any())
            return (false, "Không thể xóa vì bác sĩ đã có lịch hẹn.");

        if (doctor.MedicalRecords.Any())
            return (false, "Không thể xóa vì bác sĩ đã có hồ sơ bệnh án.");

        await _doctorRepo.DeleteAsync(doctor);
        return (true, "Xóa bác sĩ thành công.");
    }
}