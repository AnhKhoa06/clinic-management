using ClinicManagement.Data;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories;

public class AppointmentSlotRepository
{
    private readonly AppDbContext _context;

    public AppointmentSlotRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AppointmentSlot>> GetAvailableAsync(int doctorId, DateOnly date)
    {
        return await _context.AppointmentSlots
            .Include(s => s.Doctor)
                .ThenInclude(d => d.User)
            .Where(s => s.DoctorId == doctorId
                && s.SlotDate == date
                && s.Status == "Available")
            .OrderBy(s => s.SlotTime)
            .ToListAsync();
    }

    public async Task<AppointmentSlot?> GetByIdAsync(int id)
    {
        return await _context.AppointmentSlots
            .Include(s => s.Doctor)
                .ThenInclude(d => d.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> SlotExistsAsync(int doctorId, DateOnly date, TimeOnly time)
    {
        return await _context.AppointmentSlots
            .AnyAsync(s => s.DoctorId == doctorId
                && s.SlotDate == date
                && s.SlotTime == time);
    }

    public async Task AddRangeAsync(List<AppointmentSlot> slots)
    {
        await _context.AppointmentSlots.AddRangeAsync(slots);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AppointmentSlot slot)
    {
        _context.AppointmentSlots.Update(slot);
        await _context.SaveChangesAsync();
    }
}