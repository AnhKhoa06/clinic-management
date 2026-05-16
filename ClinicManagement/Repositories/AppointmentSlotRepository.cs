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

    public async Task DeleteByScheduleIdAsync(int scheduleId)
    {
        var slots = await _context.AppointmentSlots
            .Where(s => s.WorkingScheduleId == scheduleId)
            .ToListAsync();

        if (!slots.Any()) return;

        var availableSlots = slots.Where(s => s.Status == "Available").ToList();
        var bookedSlots    = slots.Where(s => s.Status == "Booked").ToList();

        // Slot Available → xóa hẳn vì chưa có ai đặt
        if (availableSlots.Any())
            _context.AppointmentSlots.RemoveRange(availableSlots);

        // Slot Booked → chỉ tách khỏi WorkingSchedule, giữ nguyên Appointment/MedicalRecord/Payment/Review
        foreach (var slot in bookedSlots)
            slot.WorkingScheduleId = null; // cần cho phép null ở model

        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasBookedSlotsAsync(int scheduleId)
    {
        return await _context.AppointmentSlots
            .AnyAsync(s => s.WorkingScheduleId == scheduleId && s.Status == "Booked");
    }

    //ktra xem lịch làm việc có slot nào đang có lịch hẹn pending/confirmed hay ko, 
    // nếu có thì không cho xóa lịch làm việc đó
    public async Task<bool> HasActiveAppointmentsAsync(int scheduleId)
    {
        var slotIds = await _context.AppointmentSlots
            .Where(s => s.WorkingScheduleId == scheduleId)
            .Select(s => s.Id)
            .ToListAsync();

        if (!slotIds.Any()) return false;

        return await _context.Appointments
            .AnyAsync(a => slotIds.Contains(a.SlotId)
                && (a.Status == "Pending" || a.Status == "Confirmed"));
    }
}