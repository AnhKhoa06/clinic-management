using System.Security.Claims;
using ClinicManagement.DTOs.Review;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[Authorize(Roles = "Patient")]
public class ReviewController : Controller
{
    private readonly ReviewService _reviewService;

    public ReviewController(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // GET /Review/Create?appointmentId=5&doctorId=2&doctorName=...
    [HttpGet]
    public IActionResult Create(int appointmentId, int doctorId, string doctorName)
    {
        var dto = new ReviewCreateDto
        {
            AppointmentId = appointmentId,
            DoctorId = doctorId
        };
        ViewBag.DoctorName = doctorName;
        return View(dto);
    }

    // POST /Review/Create
    [HttpPost]
    public async Task<IActionResult> Create(ReviewCreateDto dto, string doctorName)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.DoctorName = doctorName;
            return View(dto);
        }

        var patientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (success, message) = await _reviewService.CreateAsync(patientId, dto);

        if (!success)
        {
            ModelState.AddModelError("", message);
            ViewBag.DoctorName = doctorName;
            return View(dto);
        }

        TempData["Success"] = message;
        return RedirectToAction("MyAppointments", "Appointment");
    }
}