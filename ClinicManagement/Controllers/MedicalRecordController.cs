using ClinicManagement.DTOs.MedicalRecord;
using ClinicManagement.Helpers;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[ApiController]
[Route("api/medical-records")]
public class MedicalRecordController : ControllerBase
{
    private readonly MedicalRecordService _service;

    public MedicalRecordController(MedicalRecordService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse<List<MedicalRecordResponseDto>>.Ok(result));
    }

    [Authorize(Roles = "Admin,Doctor,Patient")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var (success, message, data) = await _service.GetByIdAsync(id);
        if (!success)
            return NotFound(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<MedicalRecordResponseDto>.Ok(data!));
    }

    [Authorize(Roles = "Doctor")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicalRecordCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Dữ liệu không hợp lệ."));

        var (success, message, data) = await _service.CreateAsync(dto);
        if (!success)
            return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<MedicalRecordResponseDto>.Ok(data!, message));
    }

    [Authorize(Roles = "Doctor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MedicalRecordUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Dữ liệu không hợp lệ."));

        var (success, message, data) = await _service.UpdateAsync(id, dto);
        if (!success)
            return BadRequest(ApiResponse<object>.Fail(message));

        return Ok(ApiResponse<MedicalRecordResponseDto>.Ok(data!, message));
    }
}