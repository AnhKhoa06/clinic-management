using ClinicManagement.DTOs.Medication;
using ClinicManagement.Helpers;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/medications")]
public class MedicationController : ControllerBase
{
    private readonly MedicationService _service;

    public MedicationController(MedicationService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse<List<MedicationResponseDto>>.Ok(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicationCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Dữ liệu không hợp lệ."));

        var (success, message, data) = await _service.CreateAsync(dto);
        if (!success)
            return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<MedicationResponseDto>.Ok(data!, message));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MedicationUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Dữ liệu không hợp lệ."));

        var (success, message, data) = await _service.UpdateAsync(id, dto);
        if (!success)
            return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<MedicationResponseDto>.Ok(data!, message));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _service.DeleteAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<object>.Ok(new { }, message));
    }
}