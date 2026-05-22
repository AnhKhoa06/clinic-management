using ClinicManagement.DTOs.Patient;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicManagement.Controllers;

[Authorize]
public class SettingsController : Controller
{
    private readonly PatientService _patientService;
    private readonly AuthService _authService;

    public SettingsController(PatientService patientService, AuthService authService)
    {
        _patientService = patientService;
        _authService = authService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Cài đặt";

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _authService.GetUserByIdAsync(userId);
        ViewBag.Phone = user?.Phone;
        ViewBag.FullName = user?.FullName;
        ViewBag.Email = user?.Email;

        if (User.IsInRole("Patient"))
        {
            var (_, _, patient) = await _patientService.GetByUserIdAsync(userId);
            ViewBag.Patient = patient;
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(string? FullName, string? Phone, string? Email, string? Gender, DateOnly? DateOfBirth, string? Address, string? EmergencyContact)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Cập nhật FullName + Phone cho tất cả roles
        if (FullName != null)
            await _authService.UpdateFullNameAsync(userId, FullName);
        if (Phone != null)
            await _authService.UpdatePhoneAsync(userId, Phone);
        
        // Tất cả roles đều được cập nhật email
        if (!string.IsNullOrWhiteSpace(Email))
        {
            var (emailSuccess, emailMessage) = await _authService.UpdateEmailAsync(userId, Email);
            if (!emailSuccess)
            {
                TempData["Error"] = emailMessage;
                return RedirectToAction(nameof(Index));
            }
        }

        // Chỉ Patient mới có Gender, DateOfBirth, Address
        if (User.IsInRole("Patient"))
        {
            var (_, _, patient) = await _patientService.GetByUserIdAsync(userId);
            if (patient != null)
            {
                var dto = new PatientUpdateDto
                {
                    Gender = Gender,
                    DateOfBirth = DateOfBirth,
                    Address = Address,
                    EmergencyContact = EmergencyContact,
                    BloodType = null
                };
                await _patientService.UpdateAsync(patient.Id, dto);
            }
        }

        TempData["Success"] = "Cập nhật thông tin thành công.";
        return RedirectToAction(nameof(Index));
    }
}