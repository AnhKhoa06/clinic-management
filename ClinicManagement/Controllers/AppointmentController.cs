using ClinicManagement.DTOs.Appointment;
using ClinicManagement.Helpers;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentController : ControllerBase
{
    private readonly AppointmentService _service;

    public AppointmentController(AppointmentService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse<List<AppointmentResponseDto>>.Ok(result));
    }

    [Authorize(Roles = "Admin,Doctor,Patient")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var (success, message, data) = await _service.GetByIdAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<AppointmentResponseDto>.Ok(data!));
    }

    [Authorize(Roles = "Patient")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppointmentCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Dữ liệu không hợp lệ."));

        var (success, message, data) = await _service.CreateAsync(dto);
        if (!success)
            return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<AppointmentResponseDto>.Ok(data!, message));
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpPatch("{id}/confirm")]
    public async Task<IActionResult> Confirm(int id)
    {
        var (success, message, data) = await _service.ConfirmAsync(id);
        if (!success)
            return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<AppointmentResponseDto>.Ok(data!, message));
    }

    [Authorize(Roles = "Admin,Doctor,Patient")]
    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id, [FromBody] AppointmentCancelDto dto)
    {
        var (success, message, data) = await _service.CancelAsync(id, dto);
        if (!success)
            return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<AppointmentResponseDto>.Ok(data!, message));
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> Complete(int id)
    {
        var (success, message, data) = await _service.CompleteAsync(id);
        if (!success)
            return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<AppointmentResponseDto>.Ok(data!, message));
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetByDoctor(int doctorId)
    {
        var result = await _service.GetByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<List<AppointmentResponseDto>>.Ok(result));
    }
}