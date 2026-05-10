using ClinicManagement.DTOs.Doctor;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[Authorize(Roles = "Admin")]
public class DoctorController : Controller
{
    private readonly DoctorService _service;
    private readonly SpecialtyService _specialtyService;

    public DoctorController(DoctorService service, SpecialtyService specialtyService)
    {
        _service = service;
        _specialtyService = specialtyService;
    }

    // GET /Doctor
    public async Task<IActionResult> Index()
    {
        var result = await _service.GetAllAsync();
        return View(result);
    }

    // GET /Doctor/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Specialties = await _specialtyService.GetAllAsync();
        return View();
    }

    // POST /Doctor/Create
    [HttpPost]
    public async Task<IActionResult> Create(DoctorCreateWithAccountDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Specialties = await _specialtyService.GetAllAsync();
            return View(dto);
        }

        var (success, message, _) = await _service.CreateWithAccountAsync(dto);
        if (!success)
        {
            ModelState.AddModelError("", message);
            ViewBag.Specialties = await _specialtyService.GetAllAsync();
            return View(dto);
        }

        TempData["Success"] = message;
        return RedirectToAction(nameof(Index));
    }

    // GET /Doctor/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var (success, _, data) = await _service.GetByIdAsync(id);
        if (!success) return NotFound();

        ViewBag.Specialties = await _specialtyService.GetAllAsync();
        ViewBag.Id = id;

        var dto = new DoctorUpdateDto
        {
            SpecialtyId = data!.SpecialtyId,
            LicenseNumber = data.LicenseNumber,
            Bio = data.Bio,
            AvatarUrl = data.AvatarUrl
        };

        return View(dto);
    }

    // POST /Doctor/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(int id, DoctorUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Specialties = await _specialtyService.GetAllAsync();
            ViewBag.Id = id;
            return View(dto);
        }

        var (success, message, _) = await _service.UpdateAsync(id, dto);
        if (!success)
        {
            ModelState.AddModelError("", message);
            ViewBag.Specialties = await _specialtyService.GetAllAsync();
            ViewBag.Id = id;
            return View(dto);
        }

        TempData["Success"] = message;
        return RedirectToAction(nameof(Index));
    }

    // POST /Doctor/ToggleActive/5
    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var (success, message) = await _service.ToggleActiveAsync(id);
        if (!success)
            TempData["Error"] = message;
        else
            TempData["Success"] = message;

        return RedirectToAction(nameof(Index));
    }
}