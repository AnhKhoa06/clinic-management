using ClinicManagement.DTOs.MedicalRecord;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicManagement.Controllers;

[Authorize]
public class MedicalRecordController : Controller
{
    private readonly MedicalRecordService _service;
    private readonly PatientService _patientService;
    private readonly MedicationService _medicationService;
    private readonly DoctorService _doctorService;

    public MedicalRecordController(
        MedicalRecordService service,
        PatientService patientService,
        MedicationService medicationService,
        DoctorService doctorService)
    {
        _service = service;
        _patientService = patientService;
        _medicationService = medicationService;
        _doctorService = doctorService;
    }

    // GET /MedicalRecord — Admin + Doctor xem tất cả
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Index()
    {
        List<MedicalRecordResponseDto> result;

        if (User.IsInRole("Admin"))
        {
            result = await _service.GetAllAsync();
        }
        else
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var doctor = await _doctorService.GetByUserIdAsync(userId);
            if (doctor == null) return View(new List<MedicalRecordResponseDto>());
            result = await _service.GetByDoctorIdAsync(doctor.Id);
        }

        return View(result);
    }

    // GET /MedicalRecord/Detail/5 — Admin + Doctor xem chi tiết
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Detail(int id)
    {
        var (success, _, data) = await _service.GetByIdAsync(id);
        if (!success) return NotFound();
        return View(data);
    }

    // GET /MedicalRecord/MyRecords — Bệnh nhân xem hồ sơ của mình
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> MyRecords()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (_, _, patient) = await _patientService.GetByUserIdAsync(userId);
        if (patient == null) return View(new List<MedicalRecordResponseDto>());

        var result = await _service.GetByPatientIdAsync(patient.Id);
        return View(result);
    }

    // GET /MedicalRecord/MyDetail/5 — Bệnh nhân xem chi tiết hồ sơ
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> MyDetail(int id)
    {
        var (success, _, data) = await _service.GetByIdAsync(id);
        if (!success) return NotFound();
        return View(data);
    }

    // GET /MedicalRecord/Create?appointmentId=1 — Doctor tạo hồ sơ
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Create(int appointmentId)
    {
        var existing = await _service.GetByAppointmentIdAsync(appointmentId);
        if (existing != null)
        {
            TempData["Error"] = "Hồ sơ bệnh án đã tồn tại, vui lòng sử dụng chức năng chỉnh sửa.";
            return RedirectToAction(nameof(Detail), new { id = existing.Id });
        }

        var medications = await _medicationService.GetAllAsync();
        ViewBag.Medications = medications;
        ViewBag.AppointmentId = appointmentId;
        return View();
    }

    // POST /MedicalRecord/Create
    [Authorize(Roles = "Doctor")]
    [HttpPost]
    public async Task<IActionResult> Create(MedicalRecordCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Medications = await _medicationService.GetAllAsync();
            ViewBag.AppointmentId = dto.AppointmentId;
            return View(dto);
        }

        var (success, message, data) = await _service.CreateAsync(dto);
        if (!success)
        {
            ModelState.AddModelError("", message);
            ViewBag.Medications = await _medicationService.GetAllAsync();
            ViewBag.AppointmentId = dto.AppointmentId;
            return View(dto);
        }

        TempData["Success"] = message;
        return RedirectToAction(nameof(Detail), new { id = data!.Id });
    }

    // GET /MedicalRecord/Edit/5
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Edit(int id)
    {
        var (success, _, data) = await _service.GetByIdAsync(id);
        if (!success) return NotFound();

        var dto = new MedicalRecordUpdateDto
        {
            Symptoms = data!.Symptoms,
            Diagnosis = data.Diagnosis,
            TreatmentNotes = data.TreatmentNotes,
            FollowUpDate = data.FollowUpDate
        };

        ViewBag.Id = id;   
        ViewBag.Record = data;
        return View(dto);
    }

    // POST /MedicalRecord/Edit/5
    [Authorize(Roles = "Doctor")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, MedicalRecordUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var (_, _, data) = await _service.GetByIdAsync(id);
            ViewBag.Id = id;   
            ViewBag.Record = data;
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
        return RedirectToAction(nameof(Detail), new { id });
    }

    // POST /MedicalRecord/Delete/5
    [Authorize(Roles = "Admin,Doctor")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _service.DeleteAsync(id);
        TempData[success ? "Success" : "Error"] = message;
        return RedirectToAction(nameof(Index));
    }
}