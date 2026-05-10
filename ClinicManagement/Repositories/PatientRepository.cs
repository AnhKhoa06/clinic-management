using ClinicManagement.Data;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories;

public class PatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Patient>> GetAllAsync()
    {
        return await _context.Patients
            .Include(p => p.User)
            .OrderBy(p => p.User.FullName)
            .ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Patient?> GetByUserIdAsync(int userId)
    {
        return await _context.Patients
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<Patient> CreateAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
    }
}