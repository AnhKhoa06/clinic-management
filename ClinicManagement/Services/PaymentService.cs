using ClinicManagement.Data;
using ClinicManagement.DTOs.Payment;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class PaymentService
{
    private readonly AppDbContext _db;
    private readonly EmailService _emailService;
    private readonly NotificationService _notificationService;

    public PaymentService(AppDbContext db, EmailService emailService, NotificationService notificationService)
    {
        _db = db;
        _emailService = emailService;
        _notificationService = notificationService;
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

        try
        {
            await _notificationService.CreateAsync(new Notification
            {
                UserId = appointment.Patient.UserId,
                Title = "Đã tạo hóa đơn",
                Message = $"Hóa đơn {invoiceCode} đã được tạo cho lịch hẹn với bác sĩ {appointment.Doctor.User.FullName}.",
                Link = $"/Payment/MyPayments"
            });

            await _notificationService.CreateAsync(new Notification
            {
                UserId = appointment.Doctor.UserId,
                Title = "Đã tạo hóa đơn",
                Message = $"Hóa đơn {invoiceCode} đã được tạo cho bệnh nhân {appointment.Patient.User.FullName}.",
                Link = $"/Payment/Detail/{payment.Id}"
            });

            await _notificationService.CreateAsync(new Notification
            {
                Role = "Admin",
                Title = "Đã tạo hóa đơn",
                Message = $"Hóa đơn {invoiceCode} đã được tạo cho bệnh nhân {appointment.Patient.User.FullName}.",
                Link = $"/Payment/Detail/{payment.Id}"
            });
        }
        catch { }

        return (true, "Tạo hóa đơn thành công.", MapToDto(payment, appointment));
    }

    // Đánh dấu đã thanh toán
    public async Task<(bool, string)> MarkPaidAsync(int id)
    {
        var payment = await _db.Payments
            .Include(p => p.Appointment).ThenInclude(a => a.Patient).ThenInclude(p => p.User)
            // lấy email, tên bệnh nhân
            .Include(p => p.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User)
            .Include(p => p.Appointment).ThenInclude(a => a.AppointmentSlot)
            .FirstOrDefaultAsync(p => p.Id == id);
            //truy vấn bất đồng bộ lấy phần tử đầu tiên thỏa điều kiện
        if (payment == null)
            return (false, "Không tìm thấy hóa đơn.");

        if (payment.Status == "Paid")
            return (false, "Hóa đơn này đã được thanh toán.");

        payment.Status = "Paid";
        payment.PaidAt = DateTime.UtcNow;// ghi lại thời điểm thanh toán
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

        try
        {
            await _notificationService.CreateAsync(new Notification
            {
                UserId = payment.Appointment.Patient.UserId,
                Title = "Thanh toán thành công",
                Message = $"Hóa đơn {payment.InvoiceCode} đã được thanh toán.",
                Link = $"/Payment/MyPayments"
            });

            await _notificationService.CreateAsync(new Notification
            {
                UserId = payment.Appointment.Doctor.UserId,
                Title = "Hóa đơn đã được thanh toán",
                Message = $"Hóa đơn {payment.InvoiceCode} của bệnh nhân {payment.Appointment.Patient.User.FullName} đã được thanh toán.",
                Link = $"/Payment/Detail/{payment.Id}"
            });

            await _notificationService.CreateAsync(new Notification
            {
                Role = "Admin",
                Title = "Đã xác nhận thanh toán",
                Message = $"Hóa đơn {payment.InvoiceCode} của bệnh nhân {payment.Appointment.Patient.User.FullName} đã được thanh toán.",
                Link = $"/Payment/Detail/{payment.Id}"
            });
        }
        catch { }

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

        try
        {
            await _notificationService.CreateAsync(new Notification
            {
                UserId = payment.Appointment.Patient.UserId,
                Title = "Thanh toán VNPay thành công",
                Message = $"Hóa đơn {payment.InvoiceCode} đã được thanh toán qua VNPay.",
                Link = $"/Payment/MyPayments"
            });

            await _notificationService.CreateAsync(new Notification
            {
                UserId = payment.Appointment.Doctor.UserId,
                Title = "Hóa đơn VNPay đã được thanh toán",
                Message = $"Hóa đơn {payment.InvoiceCode} của bệnh nhân {payment.Appointment.Patient.User.FullName} đã được thanh toán qua VNPay.",
                Link = $"/Payment/Detail/{payment.Id}"
            });

            await _notificationService.CreateAsync(new Notification
            {
                Role = "Admin",
                Title = "Đã xác nhận thanh toán VNPay",
                Message = $"Hóa đơn {payment.InvoiceCode} của bệnh nhân {payment.Appointment.Patient.User.FullName} đã được thanh toán qua VNPay.",
                Link = $"/Payment/Detail/{payment.Id}"
            });
        }
        catch { }

        return (true, "Thanh toán VNPay thành công!");
    }

    public async Task<List<PaymentResponseDto>> GetByDoctorIdAsync(int doctorId)
    {
        var payments = await _db.Payments
            .Include(p => p.Appointment).ThenInclude(a => a.Patient).ThenInclude(p => p.User)//bệnh nhân
            .Include(p => p.Appointment).ThenInclude(a => a.Doctor).ThenInclude(d => d.User)//bác sĩ
            .Include(p => p.Appointment).ThenInclude(a => a.AppointmentSlot)//ngày khám
            .Where(p => p.Appointment.DoctorId == doctorId)//Where để lọc chỉ lấy hóa đơn thuộc 
            // về đúng bs đang đn
            .OrderByDescending(p => p.Id)//mới nhất
            .ToListAsync();

        return payments.Select(p => MapToDto(p, p.Appointment)).ToList();
        //Select(MapToDto) duyệt qua từng payments trong list
    }

    public async Task<(bool, string)> SetMethodAsync(int id, string method)
    {
        var payment = await _db.Payments.FindAsync(id);//tìm hóa đơn theo id:
        if (payment == null) return (false, "Không tìm thấy hóa đơn.");
        if (payment.Status == "Paid") return (false, "Hóa đơn đã được thanh toán.");

        payment.Method = method;
        await _db.SaveChangesAsync();
        return (true, "Cập nhật phương thức thành công.");
    }

    public async Task<int> GetUnpaidCountAsync()
    {
        return await _db.Payments.CountAsync(p => p.Status == "Unpaid");
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
}