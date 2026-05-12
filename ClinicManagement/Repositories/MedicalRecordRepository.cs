using ClinicManagement.Data;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories;

public class MedicalRecordRepository
{
    private readonly AppDbContext _context;

    public MedicalRecordRepository(AppDbContext context)
    {
        _context = context;
    }

    private IQueryable<MedicalRecord> BaseQuery() =>
        _context.MedicalRecords
            .Include(mr => mr.Patient).ThenInclude(p => p.User)
            .Include(mr => mr.Doctor).ThenInclude(d => d.User)
            .Include(mr => mr.Appointment).ThenInclude(a => a.AppointmentSlot)
            .Include(mr => mr.Prescriptions).ThenInclude(p => p.Medication);

    public async Task<List<MedicalRecord>> GetAllAsync()
    {
        return await BaseQuery()
            .OrderByDescending(mr => mr.CreatedAt)
            .ToListAsync();
    }

    public async Task<MedicalRecord?> GetByIdAsync(int id)
    {
        return await BaseQuery()
            .FirstOrDefaultAsync(mr => mr.Id == id);
    }

    public async Task<List<MedicalRecord>> GetByPatientIdAsync(int patientId)
    {
        return await BaseQuery()
            .Where(mr => mr.PatientId == patientId)
            .OrderByDescending(mr => mr.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByAppointmentIdAsync(int appointmentId)
    {
        return await _context.MedicalRecords
            .AnyAsync(mr => mr.AppointmentId == appointmentId);
    }

    public async Task<MedicalRecord> CreateAsync(MedicalRecord record)
    {
        _context.MedicalRecords.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task UpdateAsync(MedicalRecord record)
    {
        _context.MedicalRecords.Update(record);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(MedicalRecord record)
    {
        _context.MedicalRecords.Remove(record);
        await _context.SaveChangesAsync();
    }

    public async Task<List<MedicalRecord>> GetByDoctorIdAsync(int doctorId)
    {
        return await BaseQuery()
            .Where(mr => mr.DoctorId == doctorId)
            .OrderByDescending(mr => mr.CreatedAt)
            .ToListAsync();
    }

    public async Task<MedicalRecord?> GetByAppointmentIdAsync(int appointmentId)
    {
        return await BaseQuery()
            .FirstOrDefaultAsync(mr => mr.AppointmentId == appointmentId);
    }
}