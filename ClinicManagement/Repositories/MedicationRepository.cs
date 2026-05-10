using ClinicManagement.Data;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories;

public class MedicationRepository
{
    private readonly AppDbContext _context;

    public MedicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Medication>> GetAllAsync()
    {
        return await _context.Medications
            .Where(m => m.IsActive)
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<Medication?> GetByIdAsync(int id)
    {
        return await _context.Medications
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
    {
        return await _context.Medications
            .AnyAsync(m => m.Name == name && m.Id != excludeId);
    }

    public async Task<Medication> CreateAsync(Medication medication)
    {
        _context.Medications.Add(medication);
        await _context.SaveChangesAsync();
        return medication;
    }

    public async Task UpdateAsync(Medication medication)
    {
        _context.Medications.Update(medication);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Medication medication)
    {
        medication.IsActive = false;
        _context.Medications.Update(medication);
        await _context.SaveChangesAsync();
    }
}