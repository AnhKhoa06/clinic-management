using ClinicManagement.DTOs.Appointment;
using ClinicManagement.Helpers;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/working-schedules")]
public class WorkingScheduleController : ControllerBase
{
    private readonly WorkingScheduleService _service;

    public WorkingScheduleController(WorkingScheduleService service)
    {
        _service = service;
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetByDoctor(int doctorId)
    {
        var result = await _service.GetByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<List<WorkingScheduleResponseDto>>.Ok(result));
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WorkingScheduleCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Dữ liệu không hợp lệ."));

        var (success, message, data) = await _service.CreateAsync(dto);
        if (!success)
            return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<WorkingScheduleResponseDto>.Ok(data!, message));
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] WorkingScheduleUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Dữ liệu không hợp lệ."));

        var (success, message, data) = await _service.UpdateAsync(id, dto);
        if (!success)
            return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<WorkingScheduleResponseDto>.Ok(data!, message));
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<object>.Ok(new { }, message));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("generate-slots")]
    public async Task<IActionResult> GenerateSlots([FromBody] GenerateSlotsDto dto)
    {
        var (success, message, count) = await _service.GenerateSlotsAsync(dto);
        if (!success)
            return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<object>.Ok(new { count }, message));
    }

    [HttpGet("slots/available")]
    public async Task<IActionResult> GetAvailableSlots([FromQuery] int doctorId, [FromQuery] DateOnly date)
    {
        var result = await _service.GetAvailableSlotsAsync(doctorId, date);
        return Ok(ApiResponse<List<AppointmentSlotResponseDto>>.Ok(result));
    }
}