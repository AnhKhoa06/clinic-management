using ClinicManagement.DTOs.Specialty;
using ClinicManagement.Models;
using ClinicManagement.Repositories;

namespace ClinicManagement.Services;

public class SpecialtyService
{
    private readonly SpecialtyRepository _repo;

    public SpecialtyService(SpecialtyRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<SpecialtyResponseDto>> GetAllAsync()
    {
        var specialties = await _repo.GetAllAsync();
        return specialties.Select(MapToDto).ToList();
    }

    public async Task<(bool Success, string Message, SpecialtyResponseDto? Data)> GetByIdAsync(int id)
    {
        var specialty = await _repo.GetByIdAsync(id);
        if (specialty == null)
            return (false, "Không tìm thấy chuyên khoa.", null);

        return (true, "OK", MapToDto(specialty));
    }

    public async Task<(bool Success, string Message, SpecialtyResponseDto? Data)> CreateAsync(SpecialtyCreateDto dto)
    {
        if (await _repo.NameExistsAsync(dto.Name))
            return (false, "Tên chuyên khoa đã tồn tại.", null);

        var specialty = new Specialty
        {
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim()
        };

        await _repo.CreateAsync(specialty);
        return (true, "Tạo chuyên khoa thành công.", MapToDto(specialty));
    }

    public async Task<(bool Success, string Message, SpecialtyResponseDto? Data)> UpdateAsync(int id, SpecialtyUpdateDto dto)
    {
        var specialty = await _repo.GetByIdAsync(id);
        if (specialty == null)
            return (false, "Không tìm thấy chuyên khoa.", null);

        if (await _repo.NameExistsAsync(dto.Name, excludeId: id))
            return (false, "Tên chuyên khoa đã tồn tại.", null);

        specialty.Name = dto.Name.Trim();
        specialty.Description = dto.Description?.Trim();

        await _repo.UpdateAsync(specialty);
        return (true, "Cập nhật thành công.", MapToDto(specialty));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var specialty = await _repo.GetByIdAsync(id);
        if (specialty == null)
            return (false, "Không tìm thấy chuyên khoa.");

        if (await _repo.HasDoctorsAsync(id))
            return (false, "Không thể xoá vì đang có bác sĩ thuộc chuyên khoa này.");

        await _repo.DeleteAsync(specialty);
        return (true, "Xoá chuyên khoa thành công.");
    }

    private static SpecialtyResponseDto MapToDto(Specialty s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        Description = s.Description,
        CreatedAt = s.CreatedAt
    };
}