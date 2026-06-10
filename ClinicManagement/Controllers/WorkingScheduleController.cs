using ClinicManagement.DTOs.Appointment;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicManagement.Controllers;

[Authorize(Roles = "Admin,Doctor")]
public class WorkingScheduleController : Controller
{
    private readonly WorkingScheduleService _service;
    private readonly DoctorService _doctorService;

    public WorkingScheduleController(WorkingScheduleService service, DoctorService doctorService)
    {
        _service = service;
        _doctorService = doctorService;
    }

    // GET /WorkingSchedule?doctorId=1
    public async Task<IActionResult> Index(int? doctorId)//nhận từ URL: /WorkingSchedule?doctorId=1
    {
        if (User.IsInRole("Doctor"))
        {
            // Doctor tự động load lịch của mình
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var doctor = await _doctorService.GetByUserIdAsync(userId);
            if (doctor == null) return View(new List<WorkingScheduleResponseDto>());

            ViewBag.DoctorId = doctor.Id;
            ViewBag.IsDoctor = true;// báo cho View biết đang là Doctor, để ẩn dropdown chọn bác sĩ đi
            var result = await _service.GetByDoctorIdAsync(doctor.Id);
            return View(result);
        }

        // Admin giữ nguyên
        var doctors = await _doctorService.GetAllAsync();
        ViewBag.Doctors = doctors;//View dùng render dropdown chọn bác sĩ
        ViewBag.DoctorId = doctorId;
        ViewBag.IsDoctor = false;//admin mới hiện dropdown

        if (doctorId == null)
        {
            var allSchedules = await _service.GetAllAsync();// Load tất cả lịch của mọi bác sĩ
            return View(allSchedules);
        }
        return View(await _service.GetByDoctorIdAsync(doctorId.Value));
    }

    // GET /WorkingSchedule/Create?doctorId=1
    public async Task<IActionResult> Create(int? doctorId)//URL query string
    {
        if (User.IsInRole("Doctor"))
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var doctor = await _doctorService.GetByUserIdAsync(userId);//tìm bác sĩ theo userId
            ViewBag.DoctorId = doctor?.Id;
            ViewBag.IsDoctor = true;//báo cho View biết đang là Doctor
            return View();
        }

        ViewBag.Doctors = await _doctorService.GetAllAsync();
        ViewBag.DoctorId = doctorId;
        ViewBag.IsDoctor = false;//admin
        return View();
    }

    // POST /WorkingSchedule/Create
    [HttpPost]
    public async Task<IActionResult> Create(WorkingScheduleCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Doctors = await _doctorService.GetAllAsync();
            return View(dto);
        }

        var (success, message, _) = await _service.CreateAsync(dto);
        if (!success)
        {
            ModelState.AddModelError("", message);
            ViewBag.Doctors = await _doctorService.GetAllAsync();
            return View(dto);
        }

        TempData["Success"] = message;
        return RedirectToAction(nameof(Index), new { doctorId = dto.DoctorId });
    }

    // GET /WorkingSchedule/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var schedule = await _service.GetByIdAsync(id);
        if (schedule == null) return NotFound();

        if (User.IsInRole("Doctor"))
        {
            ViewBag.IsDoctor = true;
            ViewBag.DoctorId = schedule.DoctorId;
        }
        else
        {
            ViewBag.IsDoctor = false;
            ViewBag.Doctors = await _doctorService.GetAllAsync();
            ViewBag.DoctorId = schedule.DoctorId;
        }

        var dto = new WorkingScheduleUpdateDto
        {
            StartTime           = schedule.StartTime,
            EndTime             = schedule.EndTime,
            SlotDurationMinutes = schedule.SlotDurationMinutes,
            IsActive            = schedule.IsActive
        };

        return View(dto);
    }

    // POST /WorkingSchedule/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(int id, int doctorId, WorkingScheduleUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Doctors = await _doctorService.GetAllAsync();
            return View(dto);
        }

        var (success, message, _) = await _service.UpdateAsync(id, dto);
        if (!success)
        {
            ModelState.AddModelError("", message);
            ViewBag.Doctors = await _doctorService.GetAllAsync();
            return View(dto);
        }

        TempData["Success"] = message;
        return RedirectToAction(nameof(Index), new { doctorId });
    }

    // POST /WorkingSchedule/Delete/5
    [HttpPost]
    public async Task<IActionResult> Delete(int id, int doctorId, bool keepFilter = false)
    {
        var (success, message) = await _service.DeleteAsync(id);
        if (!success)
            TempData["Error"] = message;
        else
            TempData["Success"] = message;

        // keepFilter=true → đang xem theo bác sĩ cụ thể → giữ doctorId
        // keepFilter=false → đang xem tất cả → về Index không kèm doctorId
        return keepFilter
            ? RedirectToAction(nameof(Index), new { doctorId })
            : RedirectToAction(nameof(Index));
    }

    // POST /WorkingSchedule/GenerateSlots
    [HttpPost]
    public async Task<IActionResult> GenerateSlots(GenerateSlotsDto dto)
    {
        TempData["LastFromDate"] = dto.FromDate.ToString("yyyy-MM-dd");
        TempData["LastToDate"]   = dto.ToDate.ToString("yyyy-MM-dd");

        var (success, message, count) = await _service.GenerateSlotsAsync(dto);
        if (!success)
            TempData["Error"] = message;
        else
            TempData["Success"] = message;

        return RedirectToAction(nameof(Index), new { doctorId = dto.DoctorId });
    }

    // GET /WorkingSchedule/Slots?doctorId=1&date=2026-05-12
    public async Task<IActionResult> Slots(int doctorId, DateOnly? date)
    {
        var doctors = await _doctorService.GetAllAsync();
        ViewBag.Doctors = doctors;
        ViewBag.DoctorId = doctorId;
        ViewBag.Date = date?.ToString("yyyy-MM-dd") ?? DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");

        if (date == null)
            date = DateOnly.FromDateTime(DateTime.Today);

        var slots = await _service.GetAvailableSlotsAsync(doctorId, date.Value);
        return View(slots);
    }
}