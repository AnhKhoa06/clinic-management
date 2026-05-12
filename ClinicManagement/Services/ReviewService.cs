using ClinicManagement.Data;
using ClinicManagement.DTOs.Review;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class ReviewService
{
    private readonly AppDbContext _db;

    public ReviewService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(bool, string)> CreateAsync(int userId, ReviewCreateDto dto)
    {
        // Tìm Patient từ UserId trước
        var patient = await _db.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
        if (patient == null)
            return (false, "Không tìm thấy thông tin bệnh nhân.");

        var appointment = await _db.Appointments
            .FirstOrDefaultAsync(a => a.Id == dto.AppointmentId
                                && a.PatientId == patient.Id  // dùng patient.Id
                                && a.Status == "Completed");

        if (appointment == null)
            return (false, "Lịch hẹn không hợp lệ hoặc chưa hoàn thành.");

        var exists = await _db.Reviews.AnyAsync(r => r.AppointmentId == dto.AppointmentId);
        if (exists)
            return (false, "Bạn đã đánh giá lịch hẹn này rồi.");

        var review = new Review
        {
            AppointmentId = dto.AppointmentId,
            PatientId = patient.Id,  // dùng patient.Id
            DoctorId = dto.DoctorId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();

        return (true, "Đánh giá thành công!");
    }

    public async Task<bool> HasReviewedAsync(int appointmentId)
    {
        return await _db.Reviews.AnyAsync(r => r.AppointmentId == appointmentId);
    }
}