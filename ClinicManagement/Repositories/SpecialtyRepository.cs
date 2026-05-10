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
            .OrderBy(s => s.Name)
            .ToListAsync();
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