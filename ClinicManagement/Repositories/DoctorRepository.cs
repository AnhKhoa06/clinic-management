using ClinicManagement.Data;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories;

public class DoctorRepository
{
    private readonly AppDbContext _context;

    public DoctorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Doctor>> GetAllAsync()
    {
        return await _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Specialty)
            .Include(d => d.Reviews)
            .OrderBy(d => d.User.FullName)
            .ToListAsync();
    }

    public async Task<Doctor?> GetByIdAsync(int id)
    {
        return await _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Specialty)
            .Include(d => d.Reviews)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<List<Doctor>> GetBySpecialtyAsync(int specialtyId)
    {
        return await _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Specialty)
            .Include(d => d.Reviews)
            .Where(d => d.SpecialtyId == specialtyId && d.IsActive)
            .OrderBy(d => d.User.FullName)
            .ToListAsync();
    }

    public async Task<bool> LicenseExistsAsync(string licenseNumber, int? excludeId = null)
    {
        return await _context.Doctors
            .AnyAsync(d => d.LicenseNumber == licenseNumber && d.Id != excludeId);
    }

    public async Task<bool> UserIdExistsAsync(int userId)
    {
        return await _context.Doctors
            .AnyAsync(d => d.UserId == userId);
    }

    public async Task<Doctor> CreateAsync(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
        return doctor;
    }

    public async Task UpdateAsync(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync();
    }

    public async Task<Doctor?> GetByUserIdAsync(int userId)
    {
        return await _context.Doctors
            .Include(d => d.User)
            .Include(d => d.Specialty)
            .Include(d => d.Reviews)
            .FirstOrDefaultAsync(d => d.UserId == userId);
    }
}