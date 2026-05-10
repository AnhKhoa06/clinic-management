using System.Security.Claims;
using ClinicManagement.DTOs.Patient;
using ClinicManagement.Helpers;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicManagement.DTOs.Appointment;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/patients")]
// [Authorize]
public class PatientController : ControllerBase
{
    private readonly PatientService _service;

    public PatientController(PatientService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse<List<PatientResponseDto>>.Ok(result));
    }

    [Authorize(Roles = "Admin,Doctor,Patient")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var (success, message, data) = await _service.GetByIdAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<PatientResponseDto>.Ok(data!));
    }

    [Authorize(Roles = "Admin,Patient")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PatientUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Dữ liệu không hợp lệ."));

        var (success, message, data) = await _service.UpdateAsync(id, dto);
        if (!success)
            return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<PatientResponseDto>.Ok(data!, message));
    }

    [Authorize(Roles = "Admin,Doctor,Patient")]
    [HttpGet("{id}/appointments")]
    public async Task<IActionResult> GetAppointments(int id)
    {
        var appointments = await _service.GetAppointmentsByPatientIdAsync(id);
        return Ok(ApiResponse<List<AppointmentResponseDto>>.Ok(appointments));
    }

    [Authorize(Roles = "Admin,Doctor,Patient")]
    [HttpGet("{id}/medical-records")]
    public async Task<IActionResult> GetMedicalRecords(int id)
    {
        var (success, message, _) = await _service.GetByIdAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<object>.Ok(new { message = "Sẽ implement sau khi có MedicalRecord module" }));
    }
}