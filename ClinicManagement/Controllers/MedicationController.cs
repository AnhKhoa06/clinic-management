using ClinicManagement.DTOs.Medication;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

public class MedicationController : Controller
{
    private readonly MedicationService _service;

    public MedicationController(MedicationService service)
    {
        _service = service;
    }

    // GET /Medication
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Index(string? search, string? unit, int page = 1)
    {
        var result = await _service.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLower();
            result = result.Where(m =>
                m.Name.ToLower().Contains(search) ||
                (m.Description != null && m.Description.ToLower().Contains(search))
            ).ToList();
        }

        if (!string.IsNullOrWhiteSpace(unit))
            result = result.Where(m => m.Unit == unit).ToList();

        var allMeds = await _service.GetAllAsync();
        ViewBag.Units = allMeds
            .Where(m => m.Unit != null)
            .Select(m => m.Unit!)
            .Distinct()
            .OrderBy(u => u)
            .ToList();

        const int pageSize = 5;
        int totalItems = result.Count;
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

        var paged = result
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.Search = search;
        ViewBag.Unit = unit;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalItems = totalItems;

        return View(paged);
    }

    // GET /Medication/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST /Medication/Create
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(MedicationCreateDto dto)
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

    // GET /Medication/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var (success, _, data) = await _service.GetByIdAsync(id);
        if (!success) return NotFound();

        ViewBag.Id = id;
        var dto = new MedicationUpdateDto
        {
            Name = data!.Name,
            Unit = data.Unit,
            Description = data.Description,
            Price = data.Price,
            IsActive = data.IsActive
        };
        return View(dto);
    }

    // POST /Medication/Edit/5
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, MedicationUpdateDto dto)
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

    // POST /Medication/Delete/5
    [Authorize(Roles = "Admin")]
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