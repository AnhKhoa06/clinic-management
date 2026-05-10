using ClinicManagement.DTOs.Specialty;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[Authorize(Roles = "Admin")]
public class SpecialtyController : Controller
{
    private readonly SpecialtyService _service;

    public SpecialtyController(SpecialtyService service)
    {
        _service = service;
    }

    // GET /Specialty
    public async Task<IActionResult> Index()
    {
        var result = await _service.GetAllAsync();
        return View(result);
    }

    // GET /Specialty/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST /Specialty/Create
    [HttpPost]
    public async Task<IActionResult> Create(SpecialtyCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var (success, message, _) = await _service.CreateAsync(dto);
        if (!success)
        {
            ModelState.AddModelError("", message);
            return View(dto);
        }

        TempData["Success"] = message;
        return RedirectToAction(nameof(Index));
    }

    // GET /Specialty/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var (success, _, data) = await _service.GetByIdAsync(id);
        if (!success)
            return NotFound();

        var dto = new SpecialtyUpdateDto
        {
            Name = data!.Name,
            Description = data.Description
        };

        ViewBag.Id = id;
        return View(dto);
    }

    // POST /Specialty/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(int id, SpecialtyUpdateDto dto)
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
        return RedirectToAction(nameof(Index));
    }

    // POST /Specialty/Delete/5
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _service.DeleteAsync(id);
        if (!success)
            TempData["Error"] = message;
        else
            TempData["Success"] = message;

        return RedirectToAction(nameof(Index));
    }
}