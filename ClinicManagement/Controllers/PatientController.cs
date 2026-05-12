using System.Security.Claims;
using ClinicManagement.DTOs.Patient;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

public class PatientController : Controller
{
    private readonly PatientService _service;

    public PatientController(PatientService service)
    {
        _service = service;
    }

    // GET /Patient — Admin + Doctor xem ds, search bệnh nhân
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Index(string? search)
    {
        var result = await _service.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLower();
            result = result.Where(p =>
                p.FullName.ToLower().Contains(search) ||
                p.Email.ToLower().Contains(search) ||
                (p.Phone != null && p.Phone.Contains(search))
            ).ToList();
        }

        ViewBag.Search = search;
        return View(result);
    }

    // GET /Patient/Detail/5
    [Authorize(Roles = "Admin,Doctor,Patient")]
    public async Task<IActionResult> Detail(int id)
    {
        var (success, _, data) = await _service.GetByIdAsync(id);
        if (!success) return NotFound();
        return View(data);
    }

    // GET /Patient/Edit/5
    [Authorize(Roles = "Admin,Patient")]
    public async Task<IActionResult> Edit(int id)
    {
        var (success, _, data) = await _service.GetByIdAsync(id);
        if (!success) return NotFound();

        var dto = new PatientUpdateDto
        {
            Gender = data!.Gender,
            DateOfBirth = data.DateOfBirth,
            Address = data.Address,
            BloodType = data.BloodType,
            EmergencyContact = data.EmergencyContact
        };

        ViewBag.Id = id;
        ViewBag.FullName = data.FullName;
        return View(dto);
    }

    // POST /Patient/Edit/5
    [Authorize(Roles = "Admin,Patient")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, PatientUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Id = id;
            return View(dto);
        }

        var (success, message, _) = await _service.UpdateAsync(id, dto);
        if (!success)
        {
            ModelState.AddModelError("", message);
            ViewBag.Id = id;
            return View(dto);
        }

        TempData["Success"] = message;

        // Nếu là Patient thì redirect về profile, Admin thì về danh sách
        if (User.IsInRole("Admin"))
            return RedirectToAction(nameof(Index));

        return RedirectToAction(nameof(Detail), new { id });
    }

    // GET /Patient/MyProfile — Patient xem profile của mình
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> MyProfile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (success, _, data) = await _service.GetByUserIdAsync(userId);
        if (!success) return NotFound();
        return View(data);
    }
}