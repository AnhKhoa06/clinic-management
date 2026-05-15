using ClinicManagement.Data;
using ClinicManagement.DTOs.Payment;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class PaymentService
{
    private readonly AppDbContext _db;
    private readonly EmailService _emailService;

    public PaymentService(AppDbContext db, EmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }
    // Admin/Doctor tạo hóa đơn
    public async Task<(bool, string, PaymentResponseDto?)> CreateAsync(PaymentCreateDto dto)
    {
        var appointment = await _db.Appointments
            .Include(a => a.Patient).ThenInclude(p => p.User)
            .Include(a => a.Doctor).ThenInclude(d => d.User)
            .Include(a => a.AppointmentSlot)
            .FirstOrDefaultAsync(a => a.Id == dto.AppointmentId && a.Status == "Completed");

        if (appointment == null)
            return (false, "Lịch hẹn không hợp lệ hoặc chưa hoàn thành.", null);

        var exists = await _db.Payments.AnyAsync(p => p.AppointmentId == dto.AppointmentId);
        if (exists)
            return (false, "Lịch hẹn này đã có hóa đơn rồi.", null);

        // Lấy hồ sơ bệnh án + đơn thuốc
        var medicalRecord = await _db.MedicalRecords
            .Include(mr => mr.Prescriptions)
            .FirstOrDefaultAsync(mr => mr.AppointmentId == dto.AppointmentId);

        // Tính phí
        decimal examinationFee = appointment.Doctor.ExaminationFee;
        decimal medicationFee = medicalRecord?.Prescriptions
            .Sum(p => p.Quantity * p.UnitPrice) ?? 0;
        decimal amount = examinationFee + medicationFee;

        var invoiceCode = $"INV-{DateTime.Now:yyyyMMdd}-{dto.AppointmentId:D4}";

        var payment = new Payment
        {
            AppointmentId = dto.AppointmentId,
            InvoiceCode = invoiceCode,
            ExaminationFee = examinationFee,
            MedicationFee = medicationFee,
            Amount = amount,
            Status = "Unpaid",
            Method = dto.Method,
            Notes = dto.Notes
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        return (true, "Tạo hóa đơn thành công.", MapToDto(payment, appointment));
    }

    // Đánh dấu đã thanh toán
    public async Task<(bool, string)> MarkPaidAsync(int id)
    {
        var payment = await _db.Payments
            .Include(p => p.Appointment).ThenInclude(a => a.Patient).ThenInclude(p => p.User)
            .Include(p => p.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User)
            .Include(p => p.Appointment).ThenInclude(a => a.AppointmentSlot)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (payment == null)
            return (false, "Không tìm thấy hóa đơn.");

        if (payment.Status == "Paid")
            return (false, "Hóa đơn này đã được thanh toán.");

        payment.Status = "Paid";
        payment.PaidAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        // Gửi email thông báo
        await _emailService.SendPaymentSuccessAsync(
            toEmail:     payment.Appointment.Patient.User.Email,
            patientName: payment.Appointment.Patient.User.FullName,
            invoiceCode: payment.InvoiceCode,
            doctorName:  payment.Appointment.Doctor.User.FullName,
            slotDate:    payment.Appointment.AppointmentSlot.SlotDate.ToString("dd/MM/yyyy"),
            amount:      payment.Amount,
            method:      payment.Method == "Cash" ? "Tiền mặt" :
                        payment.Method == "BankTransfer" ? "Chuyển khoản" : payment.Method
        );

        return (true, "Thanh toán thành công!");
    }

    // Lấy tất cả (Admin)
    public async Task<List<PaymentResponseDto>> GetAllAsync()
    {
        var payments = await _db.Payments
            .Include(p => p.Appointment).ThenInclude(a => a.Patient).ThenInclude(p => p.User)
            .Include(p => p.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User)
            .Include(p => p.Appointment).ThenInclude(a => a.AppointmentSlot)
            .OrderByDescending(p => p.Id)
            .ToListAsync();

        return payments.Select(p => MapToDto(p, p.Appointment)).ToList();
    }

    // Lấy theo bệnh nhân
    public async Task<List<PaymentResponseDto>> GetByPatientAsync(int patientId)
    {
        var payments = await _db.Payments
            .Include(p => p.Appointment).ThenInclude(a => a.Patient).ThenInclude(p => p.User)
            .Include(p => p.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User)
            .Include(p => p.Appointment).ThenInclude(a => a.AppointmentSlot)
            .Where(p => p.Appointment.Patient.UserId == patientId)
            .OrderByDescending(p => p.Id)
            .ToListAsync();

        return payments.Select(p => MapToDto(p, p.Appointment)).ToList();
    }

    public async Task<(bool, string, PaymentResponseDto?)> GetByIdAsync(int id)
    {
        var p = await _db.Payments
            .Include(p => p.Appointment).ThenInclude(a => a.Patient).ThenInclude(p => p.User)
            .Include(p => p.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User)
            .Include(p => p.Appointment).ThenInclude(a => a.AppointmentSlot)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (p == null) return (false, "Không tìm thấy hóa đơn.", null);
        return (true, "OK", MapToDto(p, p.Appointment));
    }

    private static PaymentResponseDto MapToDto(Payment p, Appointment a) => new()
    {
        Id = p.Id,
        AppointmentId = p.AppointmentId,
        InvoiceCode = p.InvoiceCode,
        PatientName = a.Patient.User.FullName,
        DoctorName = a.Doctor.User.FullName,
        SlotDate = a.AppointmentSlot.SlotDate,
        ExaminationFee = p.ExaminationFee,
        MedicationFee = p.MedicationFee,
        Amount = p.Amount,
        Status = p.Status,
        Method = p.Method,
        Notes = p.Notes,
        PaidAt = p.PaidAt,
        CreatedAt = p.CreatedAt
    };

    public async Task<(bool, string)> MarkPaidVnPayAsync(int paymentId)
    {
        var payment = await _db.Payments
            .Include(p => p.Appointment).ThenInclude(a => a.Patient).ThenInclude(p => p.User)
            .Include(p => p.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User)
            .Include(p => p.Appointment).ThenInclude(a => a.AppointmentSlot)
            .FirstOrDefaultAsync(p => p.Id == paymentId);

        if (payment == null)
            return (false, "Không tìm thấy hóa đơn.");

        if (payment.Status == "Paid")
            return (false, "Hóa đơn đã được thanh toán.");

        payment.Status = "Paid";
        payment.PaidAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        // Gửi email thông báo
        await _emailService.SendPaymentSuccessAsync(
            toEmail:     payment.Appointment.Patient.User.Email,
            patientName: payment.Appointment.Patient.User.FullName,
            invoiceCode: payment.InvoiceCode,
            doctorName:  payment.Appointment.Doctor.User.FullName,
            slotDate:    payment.Appointment.AppointmentSlot.SlotDate.ToString("dd/MM/yyyy"),
            amount:      payment.Amount,
            method:      "Chuyển khoản (VNPay)"
        );

        return (true, "Thanh toán VNPay thành công!");
    }

    public async Task<List<PaymentResponseDto>> GetByDoctorIdAsync(int doctorId)
    {
        var payments = await _db.Payments
            .Include(p => p.Appointment).ThenInclude(a => a.Patient).ThenInclude(p => p.User)
            .Include(p => p.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User)
            .Include(p => p.Appointment).ThenInclude(a => a.AppointmentSlot)
            .Where(p => p.Appointment.DoctorId == doctorId)
            .OrderByDescending(p => p.Id)
            .ToListAsync();

        return payments.Select(p => MapToDto(p, p.Appointment)).ToList();
    }
}