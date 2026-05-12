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

        var slotIds = slots.Select(s => s.Id).ToList();

        var appointments = await _context.Appointments
            .Where(a => slotIds.Contains(a.SlotId))
            .ToListAsync();

        if (appointments.Any())
        {
            var appointmentIds = appointments.Select(a => a.Id).ToList();

            // Prescriptions (con của MedicalRecord)
            var medicalRecordIds = await _context.MedicalRecords
                .Where(m => appointmentIds.Contains(m.AppointmentId))
                .Select(m => m.Id)
                .ToListAsync();

            if (medicalRecordIds.Any())
            {
                var prescriptions = await _context.Prescriptions
                    .Where(p => medicalRecordIds.Contains(p.MedicalRecordId))
                    .ToListAsync();
                if (prescriptions.Any())
                    _context.Prescriptions.RemoveRange(prescriptions);
            }

            // MedicalRecords
            var medicalRecords = await _context.MedicalRecords
                .Where(m => appointmentIds.Contains(m.AppointmentId))
                .ToListAsync();
            if (medicalRecords.Any())
                _context.MedicalRecords.RemoveRange(medicalRecords);

            // Reviews
            var reviews = await _context.Reviews
                .Where(r => appointmentIds.Contains(r.AppointmentId))
                .ToListAsync();
            if (reviews.Any())
                _context.Reviews.RemoveRange(reviews);

            // Payments
            var payments = await _context.Payments
                .Where(p => appointmentIds.Contains(p.AppointmentId))
                .ToListAsync();
            if (payments.Any())
                _context.Payments.RemoveRange(payments);

            // Appointments
            _context.Appointments.RemoveRange(appointments);
        }

        // AppointmentSlots
        _context.AppointmentSlots.RemoveRange(slots);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasBookedSlotsAsync(int scheduleId)
    {
        return await _context.AppointmentSlots
            .AnyAsync(s => s.WorkingScheduleId == scheduleId && s.Status == "Booked");
    }
}