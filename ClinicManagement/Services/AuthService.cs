using ClinicManagement.DTOs.Auth;
using ClinicManagement.Helpers;
using ClinicManagement.Models;
using ClinicManagement.Repositories;
using ClinicManagement.Data;

namespace ClinicManagement.Services;

public class AuthService
{
    private readonly AuthRepository _authRepository;
    private readonly AppDbContext _context;

    public AuthService(
        AuthRepository authRepository,
        AppDbContext context)
    {
        _authRepository = authRepository;
        _context = context;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto)
    {
        if (await _authRepository.EmailExistsAsync(dto.Email))
            return (false, "Email đã được sử dụng.");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email.ToLower().Trim(),
            PasswordHash = PasswordHelper.HashPassword(dto.Password),
            Phone = dto.Phone,
            Role = "Patient"
        };

        await _authRepository.CreateUserAsync(user);

        var patient = new Patient { UserId = user.Id };
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        return (true, "Đăng ký thành công.");
    }

    public async Task<(bool Success, string Message, User? Data)> LoginAsync(LoginDto dto)
    {
        var user = await _authRepository.GetUserByEmailAsync(dto.Email.ToLower().Trim());

        if (user == null || !PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash))
            return (false, "Email hoặc mật khẩu không đúng.", null);

        if (!user.IsActive)
            return (false, "Tài khoản đã bị khoá.", null);

        return (true, "Đăng nhập thành công.", user);
    }

    public async Task<(bool Success, string Message, User? Data)> LoginWithGoogleAsync(string email, string fullName)
    {
        var user = await _authRepository.GetUserByEmailAsync(email.ToLower().Trim());

        if (user == null)
        {
            // Tự động tạo tài khoản mới nếu chưa có
            user = new User
            {
                FullName = fullName,
                Email = email.ToLower().Trim(),
                PasswordHash = PasswordHelper.HashPassword(Guid.NewGuid().ToString()),
                Phone = "",
                Role = "Patient",
                IsActive = true
            };

            await _authRepository.CreateUserAsync(user);

            var patient = new Patient { UserId = user.Id };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        if (!user.IsActive)
            return (false, "Tài khoản đã bị khoá.", null);

        return (true, "Đăng nhập thành công.", user);
    }

    public async Task<(bool Success, string Message)> ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await _authRepository.GetUserByIdAsync(userId);
        if (user == null)
            return (false, "Không tìm thấy người dùng.");

        if (!PasswordHelper.VerifyPassword(dto.CurrentPassword, user.PasswordHash))
            return (false, "Mật khẩu hiện tại không đúng.");

        user.PasswordHash = PasswordHelper.HashPassword(dto.NewPassword);
        await _authRepository.UpdateUserAsync(user);
        return (true, "Đổi mật khẩu thành công.");
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _authRepository.GetUserByIdAsync(id);
    }

    public async Task UpdatePhoneAsync(int userId, string phone)
    {
        var user = await _authRepository.GetUserByIdAsync(userId);
        if (user == null) return;

        user.Phone = phone.Trim();
        await _authRepository.UpdateUserAsync(user);
    }
    public async Task UpdateFullNameAsync(int userId, string fullName)
    {
        var user = await _authRepository.GetUserByIdAsync(userId);
        if (user == null) return;

        user.FullName = fullName.Trim();
        await _authRepository.UpdateUserAsync(user);
    }
}