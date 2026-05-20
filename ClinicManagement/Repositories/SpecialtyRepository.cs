using ClinicManagement.Data;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories;

public class SpecialtyRepository
{
    private readonly AppDbContext _context;

    public SpecialtyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Specialty>> GetAllAsync()
    {
        return await _context.Specialties
            .OrderBy(s => s.Name)//sx kết quả theo cột name với thứ tự tăng dần (a-z)
            .ToListAsync();//Trả về các rows kết quả, --> quay lại repository: 
            // EF Core nhận rows từ MySQL, map từng row thành object Specialty, 
            // gộp thành List<Speciatly> và trả về cho service
    }

    public async Task<Specialty?> GetByIdAsync(int id)
    {
        return await _context.Specialties
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
    {
        return await _context.Specialties
            .AnyAsync(s => s.Name == name && s.Id != excludeId);
    }

    public async Task<Specialty> CreateAsync(Specialty specialty)
    {
        _context.Specialties.Add(specialty);
        await _context.SaveChangesAsync();
        return specialty;
    }

    public async Task UpdateAsync(Specialty specialty)
    {
        _context.Specialties.Update(specialty);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Specialty specialty)
    {
        _context.Specialties.Remove(specialty);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasDoctorsAsync(int specialtyId)
    {
        return await _context.Doctors
            .AnyAsync(d => d.SpecialtyId == specialtyId);
    }
}