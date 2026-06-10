using Microsoft.EntityFrameworkCore;
using ClinicManagement.Models;

namespace ClinicManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<Doctor> Doctors { get; set; }// tương ứng bảng Doctors trong DB
    public DbSet<Patient> Patients { get; set; }
    public DbSet<WorkingSchedule> WorkingSchedules { get; set; }
    public DbSet<AppointmentSlot> AppointmentSlots { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<Medication> Medications { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {   
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Role)
                .HasDefaultValue("Patient");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasIndex(d => d.LicenseNumber).IsUnique();
            entity.HasOne(d => d.User)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(d => d.Specialty)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecialtyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasOne(p => p.User)
                .WithOne(u => u.Patient)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WorkingSchedule>(entity =>
        {
            entity.HasOne(ws => ws.Doctor)
                .WithMany(d => d.WorkingSchedules)
                .HasForeignKey(ws => ws.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AppointmentSlot>(entity =>
        {
            entity.HasIndex(s => new { s.DoctorId, s.SlotDate, s.SlotTime }).IsUnique();
            entity.HasOne(s => s.WorkingSchedule)
                .WithMany(ws => ws.AppointmentSlots)
                .HasForeignKey(s => s.WorkingScheduleId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            entity.HasOne(s => s.Doctor)
                .WithMany()
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(a => a.AppointmentSlot)
                .WithMany()
                .HasForeignKey(a => a.SlotId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasOne(mr => mr.Appointment)
                .WithOne(a => a.MedicalRecord)
                .HasForeignKey<MedicalRecord>(mr => mr.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(mr => mr.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(mr => mr.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(mr => mr.Doctor)
                .WithMany(d => d.MedicalRecords)
                .HasForeignKey(mr => mr.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasOne(p => p.MedicalRecord)
                .WithMany(mr => mr.Prescriptions)
                .HasForeignKey(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(p => p.Medication)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(p => p.MedicationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasIndex(p => p.InvoiceCode).IsUnique();
            entity.HasOne(p => p.Appointment)
                .WithOne(a => a.Payment)
                .HasForeignKey<Payment>(p => p.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasIndex(r => r.AppointmentId).IsUnique();
            entity.HasOne(r => r.Appointment)
                .WithOne(a => a.Review)
                .HasForeignKey<Review>(r => r.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(r => r.Patient)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(r => r.Doctor)
                .WithMany(d => d.Reviews)
                .HasForeignKey(r => r.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasIndex(n => new { n.UserId, n.IsRead });
            entity.Property(n => n.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}