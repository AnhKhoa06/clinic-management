using ClinicManagement.DTOs.Medication;
using ClinicManagement.Models;
using ClinicManagement.Repositories;

namespace ClinicManagement.Services;

public class MedicationService
{
    private readonly MedicationRepository _repo;

    public MedicationService(MedicationRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<MedicationResponseDto>> GetAllAsync()
    {
        var medications = await _repo.GetAllAsync();
        return medications.Select(MapToDto).ToList();
    }

    public async Task<(bool Success, string Message, MedicationResponseDto? Data)> CreateAsync(MedicationCreateDto dto)
    {
        if (await _repo.NameExistsAsync(dto.Name))
            return (false, "Tên thuốc đã tồn tại.", null);

        var medication = new Medication
        {
            Name = dto.Name.Trim(),
            Unit = dto.Unit?.Trim(),
            Description = dto.Description?.Trim(),
            IsActive = true
        };

        await _repo.CreateAsync(medication);
        return (true, "Thêm thuốc thành công.", MapToDto(medication));
    }

    public async Task<(bool Success, string Message, MedicationResponseDto? Data)> UpdateAsync(int id, MedicationUpdateDto dto)
    {
        var medication = await _repo.GetByIdAsync(id);
        if (medication == null)
            return (false, "Không tìm thấy thuốc.", null);

        if (await _repo.NameExistsAsync(dto.Name, excludeId: id))
            return (false, "Tên thuốc đã tồn tại.", null);

        medication.Name = dto.Name.Trim();
        medication.Unit = dto.Unit?.Trim();
        medication.Description = dto.Description?.Trim();
        medication.IsActive = dto.IsActive;

        await _repo.UpdateAsync(medication);
        return (true, "Cập nhật thuốc thành công.", MapToDto(medication));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var medication = await _repo.GetByIdAsync(id);
        if (medication == null)
            return (false, "Không tìm thấy thuốc.");

        await _repo.DeleteAsync(medication);
        return (true, "Xoá thuốc thành công.");
    }

    private static MedicationResponseDto MapToDto(Medication m) => new()
    {
        Id = m.Id,
        Name = m.Name,
        Unit = m.Unit,
        Description = m.Description,
        IsActive = m.IsActive
    };
}