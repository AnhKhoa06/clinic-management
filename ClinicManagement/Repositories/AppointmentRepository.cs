using ClinicManagement.Data;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories;

public class AppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }

    private IQueryable<Appointment> BaseQuery() =>
        _context.Appointments
            .Include(a => a.Patient).ThenInclude(p => p.User)
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Include(a => a.Doctor).ThenInclude(d => d.Specialty)
            .Include(a => a.AppointmentSlot);

    public async Task<List<Appointment>> GetAllAsync()
    {
        return await BaseQuery()
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await BaseQuery()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Appointment>> GetByPatientIdAsync(int patientId)
    {
        return await BaseQuery()
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetByDoctorIdAsync(int doctorId)
    {
        return await BaseQuery()
            .Where(a => a.DoctorId == doctorId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Appointment> CreateAsync(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }
}