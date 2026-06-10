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
            .OrderBy(ws => ws.DayOfWeek)//Sắp xếp theo thứ trong tuần - thứ 2 lên trên chủ nhật xuống dưới.
            .ToListAsync();//thực thi câu truy vấn bất đồng bộ 
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
            .ToListAsync();//thực thi câu truy vấn bất đồng bộ 
    }

    public async Task<bool> ExistsAsync(int doctorId, int dayOfWeek, int? excludeId = null)
    {
        return await _context.WorkingSchedules
            .AnyAsync(ws => ws.DoctorId == doctorId//Lọc đúng bác sĩ đang tạo lịch
                && ws.DayOfWeek == dayOfWeek
                && ws.Id != excludeId
                && ws.IsActive);
    }//AnyAsync — kiểm tra có tồn tại bất kỳ dòng nào thỏa điều kiện không ,trả về true/false

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
            .ToListAsync();//thực thi câu truy vấn bất đồng bộ 
    }
}