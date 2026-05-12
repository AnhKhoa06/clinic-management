using ClinicManagement.DTOs.Appointment;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicManagement.Controllers;

[Authorize]
public class AppointmentController : Controller
{
    private readonly AppointmentService _appointmentService;
    private readonly DoctorService _doctorService;
    private readonly PatientService _patientService;
    private readonly WorkingScheduleService _scheduleService;

    public AppointmentController(
        AppointmentService appointmentService,
        DoctorService doctorService,
        PatientService patientService,
        WorkingScheduleService scheduleService)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _patientService = patientService;
        _scheduleService = scheduleService;
    }

    // GET /Appointment — Admin + Doctor xem tất cả
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Index(string? status, DateOnly? date)
    {
        List<AppointmentResponseDto> result;

        if (User.IsInRole("Doctor"))
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var doctor = await _doctorService.GetByUserIdAsync(userId);
            if (doctor == null) return View(new List<AppointmentResponseDto>());
            result = await _appointmentService.GetByDoctorIdAsync(doctor.Id);
        }
        else
        {
            result = await _appointmentService.GetAllAsync();
        }

        if (!string.IsNullOrEmpty(status))
            result = result.Where(a => a.Status == status).ToList();

        if (date.HasValue)
            result = result.Where(a => a.SlotDate == date.Value).ToList();

        ViewBag.Status = status;
        ViewBag.Date = date?.ToString("yyyy-MM-dd");
        ViewBag.StatusList = new List<string> { "Pending", "Confirmed", "Completed", "Cancelled" };
        return View(result);
    }

    // GET /Appointment/MyAppointments — Bệnh nhân xem lịch của mình
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> MyAppointments()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (_, _, patient) = await _patientService.GetByUserIdAsync(userId);
        if (patient == null) return View(new List<AppointmentResponseDto>());

        var result = await _appointmentService.GetByPatientIdAsync(patient.Id);
        return View(result);
    }

    // GET /Appointment/Create — Bệnh nhân đặt lịch
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Doctors = await _doctorService.GetAllAsync();
        return View();
    }

    // POST /Appointment/Create
    [Authorize(Roles = "Patient")]
    [HttpPost]
    public async Task<IActionResult> Create(AppointmentCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Doctors = await _doctorService.GetAllAsync();
            return View(dto);
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (_, _, patient) = await _patientService.GetByUserIdAsync(userId);
        if (patient == null)
        {
            ModelState.AddModelError("", "Không tìm thấy thông tin bệnh nhân.");
            ViewBag.Doctors = await _doctorService.GetAllAsync();
            return View(dto);
        }

        dto.PatientId = patient.Id;

        var (success, message, _) = await _appointmentService.CreateAsync(dto);
        if (!success)
        {
            ModelState.AddModelError("", message);
            ViewBag.Doctors = await _doctorService.GetAllAsync();
            return View(dto);
        }

        TempData["Success"] = message;
        return RedirectToAction(nameof(MyAppointments));
    }

    // POST /Appointment/Confirm/5
    [Authorize(Roles = "Admin,Doctor")]
    [HttpPost]
    public async Task<IActionResult> Confirm(int id)
    {
        var (success, message, _) = await _appointmentService.ConfirmAsync(id);
        if (!success) TempData["Error"] = message;
        else TempData["Success"] = message;
        return RedirectToAction(nameof(Index));
    }

    // POST /Appointment/Complete/5
    [Authorize(Roles = "Admin,Doctor")]
    [HttpPost]
    public async Task<IActionResult> Complete(int id)
    {
        var (success, message, _) = await _appointmentService.CompleteAsync(id);
        if (!success) TempData["Error"] = message;
        else TempData["Success"] = message;
        return RedirectToAction(nameof(Index));
    }

    // GET /Appointment/Cancel/5
    public async Task<IActionResult> Cancel(int id)
    {
        var (success, _, data) = await _appointmentService.GetByIdAsync(id);
        if (!success) return NotFound();
        return View(data);
    }

    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Detail(int id)
    {
        var (success, _, data) = await _appointmentService.GetByIdAsync(id);
        if (!success) return NotFound();
        return View(data);
    }

    // POST /Appointment/Cancel/5
    [HttpPost]
    public async Task<IActionResult> CancelConfirm(int id, AppointmentCancelDto dto)
    {
        var (success, message, _) = await _appointmentService.CancelAsync(id, dto);
        if (!success) TempData["Error"] = message;
        else TempData["Success"] = message;

        return User.IsInRole("Patient")
            ? RedirectToAction(nameof(MyAppointments))
            : RedirectToAction(nameof(Index));
    }

    // GET /Appointment/GetSlots?doctorId=1&date=2026-05-15 — AJAX
    [HttpGet]
    public async Task<IActionResult> GetSlots(int doctorId, DateOnly date)
    {
        var slots = await _scheduleService.GetAvailableSlotsAsync(doctorId, date);
        return Json(slots);
    }

    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> MyDetail(int id)
    {
        var (success, _, data) = await _appointmentService.GetByIdAsync(id);
        if (!success) return NotFound();
        return View(data);
    }

    // POST /Appointment/Delete/5
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _appointmentService.DeleteAsync(id);
        TempData[success ? "Success" : "Error"] = message;
        return RedirectToAction(nameof(Index));
    }
}