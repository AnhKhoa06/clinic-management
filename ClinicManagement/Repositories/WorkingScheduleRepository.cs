using ClinicManagement.Data;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories;

public class WorkingScheduleRepository
{
    private readonly AppDbContext _context;

    public WorkingScheduleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkingSchedule>> GetByDoctorIdAsync(int doctorId)
    {
        return await _context.WorkingSchedules
            .Include(ws => ws.Doctor)
                .ThenInclude(d => d.User)
            .Where(ws => ws.DoctorId == doctorId)
            .OrderBy(ws => ws.DayOfWeek)
            .ToListAsync();
    }

    public async Task<WorkingSchedule?> GetByIdAsync(int id)
    {
        return await _context.WorkingSchedules
            .Include(ws => ws.Doctor)
                .ThenInclude(d => d.User)
            .FirstOrDefaultAsync(ws => ws.Id == id);
    }

    public async Task<List<WorkingSchedule>> GetAllAsync()
    {
        return await _context.WorkingSchedules
            .Include(ws => ws.Doctor)
                .ThenInclude(d => d.User)
            .OrderBy(ws => ws.DoctorId)
            .ThenBy(ws => ws.DayOfWeek)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(int doctorId, int dayOfWeek, int? excludeId = null)
    {
        return await _context.WorkingSchedules
            .AnyAsync(ws => ws.DoctorId == doctorId
                && ws.DayOfWeek == dayOfWeek
                && ws.Id != excludeId
                && ws.IsActive);
    }

    public async Task<WorkingSchedule> CreateAsync(WorkingSchedule schedule)
    {
        _context.WorkingSchedules.Add(schedule);
        await _context.SaveChangesAsync();
        return schedule;
    }

    public async Task UpdateAsync(WorkingSchedule schedule)
    {
        _context.WorkingSchedules.Update(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(WorkingSchedule schedule)
    {
        _context.WorkingSchedules.Remove(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task<List<WorkingSchedule>> GetActiveByDoctorIdAsync(int doctorId)
    {
        return await _context.WorkingSchedules
            .Where(ws => ws.DoctorId == doctorId && ws.IsActive)
            .ToListAsync();
    }
}