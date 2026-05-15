using ClinicManagement.DTOs.Appointment;
using ClinicManagement.Models;
using ClinicManagement.Repositories;

namespace ClinicManagement.Services;

public class WorkingScheduleService
{
    private readonly WorkingScheduleRepository _scheduleRepo;
    private readonly AppointmentSlotRepository _slotRepo;

    public WorkingScheduleService(
        WorkingScheduleRepository scheduleRepo,
        AppointmentSlotRepository slotRepo)
    {
        _scheduleRepo = scheduleRepo;
        _slotRepo = slotRepo;
    }

    public async Task<List<WorkingScheduleResponseDto>> GetByDoctorIdAsync(int doctorId)
    {
        var schedules = await _scheduleRepo.GetByDoctorIdAsync(doctorId);
        return schedules.Select(MapToDto).ToList();
    }

    public async Task<List<WorkingScheduleResponseDto>> GetAllAsync()
    {
        var schedules = await _scheduleRepo.GetAllAsync();
        return schedules.Select(MapToDto).ToList();
    }

    public async Task<(bool, string, WorkingScheduleResponseDto?)> CreateAsync(WorkingScheduleCreateDto dto)
    {
        if (dto.StartTime >= dto.EndTime)
            return (false, "Giờ bắt đầu phải trước giờ kết thúc.", null);

        if (await _scheduleRepo.ExistsAsync(dto.DoctorId, dto.DayOfWeek))
            return (false, "Bác sĩ đã có lịch làm việc vào ngày này.", null);

        // Tự tính MaxSlots
        int maxSlots = (int)((dto.EndTime - dto.StartTime).TotalMinutes / dto.SlotDurationMinutes);

        var schedule = new WorkingSchedule
        {
            DoctorId            = dto.DoctorId,
            DayOfWeek           = dto.DayOfWeek,
            StartTime           = dto.StartTime,
            EndTime             = dto.EndTime,
            SlotDurationMinutes = dto.SlotDurationMinutes,
            MaxSlots            = maxSlots, // tự tính
            IsActive            = true
        };

        await _scheduleRepo.CreateAsync(schedule);
        var created = await _scheduleRepo.GetByIdAsync(schedule.Id);
        return (true, "Tạo lịch làm việc thành công.", MapToDto(created!));
    }

    public async Task<(bool, string, WorkingScheduleResponseDto?)> UpdateAsync(int id, WorkingScheduleUpdateDto dto)
    {
        var schedule = await _scheduleRepo.GetByIdAsync(id);
        if (schedule == null)
            return (false, "Không tìm thấy lịch làm việc.", null);

        if (dto.StartTime >= dto.EndTime)
            return (false, "Giờ bắt đầu phải trước giờ kết thúc.", null);

        schedule.StartTime           = dto.StartTime;
        schedule.EndTime             = dto.EndTime;
        schedule.SlotDurationMinutes = dto.SlotDurationMinutes;
        schedule.MaxSlots            = (int)((dto.EndTime - dto.StartTime).TotalMinutes / dto.SlotDurationMinutes); // tự tính
        schedule.IsActive            = dto.IsActive;

        await _scheduleRepo.UpdateAsync(schedule);
        return (true, "Cập nhật lịch làm việc thành công.", MapToDto(schedule));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var schedule = await _scheduleRepo.GetByIdAsync(id);
        if (schedule == null)
            return (false, "Không tìm thấy lịch làm việc.");

        await _slotRepo.DeleteByScheduleIdAsync(id);
        await _scheduleRepo.DeleteAsync(schedule);
        return (true, "Xoá lịch làm việc thành công.");
    }

    public async Task<(bool Success, string Message, int Count)> GenerateSlotsAsync(GenerateSlotsDto dto)
    {
        var schedules = await _scheduleRepo.GetActiveByDoctorIdAsync(dto.DoctorId);
        if (!schedules.Any())
            return (false, "Bác sĩ chưa có lịch làm việc.", 0);

        var slots = new List<AppointmentSlot>();
        var current = dto.FromDate;

        while (current <= dto.ToDate)
        {
            var dayOfWeek = (int)current.DayOfWeek;
            var schedule = schedules.FirstOrDefault(s => s.DayOfWeek == dayOfWeek);

            if (schedule != null)
            {
                var slotTime = schedule.StartTime;
                while (slotTime < schedule.EndTime)
                {
                    var exists = await _slotRepo.SlotExistsAsync(dto.DoctorId, current, slotTime);
                    if (!exists)
                    {
                        slots.Add(new AppointmentSlot
                        {
                            WorkingScheduleId = schedule.Id,
                            DoctorId = dto.DoctorId,
                            SlotDate = current,
                            SlotTime = slotTime,
                            Status = "Available"
                        });
                    }
                    slotTime = slotTime.AddMinutes(schedule.SlotDurationMinutes);
                }
            }
            current = current.AddDays(1);
        }

        if (slots.Any())
            await _slotRepo.AddRangeAsync(slots);

        return (true, $"Đã tạo {slots.Count} slots thành công.", slots.Count);
    }

    public async Task<List<AppointmentSlotResponseDto>> GetAvailableSlotsAsync(int doctorId, DateOnly date)
    {
        var slots = await _slotRepo.GetAvailableAsync(doctorId, date);
        return slots.Select(s => new AppointmentSlotResponseDto
        {
            Id = s.Id,
            DoctorId = s.DoctorId,
            DoctorName = s.Doctor.User.FullName,
            SlotDate = s.SlotDate,
            SlotTime = s.SlotTime,
            Status = s.Status
        }).ToList();
    }

    private static WorkingScheduleResponseDto MapToDto(WorkingSchedule ws) => new()
    {
        Id = ws.Id,
        DoctorId = ws.DoctorId,
        DoctorName = ws.Doctor.User.FullName,
        DayOfWeek = ws.DayOfWeek,
        DayOfWeekName = ws.DayOfWeek switch
        {
            0 => "Chủ nhật",
            1 => "Thứ hai",
            2 => "Thứ ba",
            3 => "Thứ tư",
            4 => "Thứ năm",
            5 => "Thứ sáu",
            6 => "Thứ bảy",
            _ => "Không xác định"
        },
        StartTime = ws.StartTime,
        EndTime = ws.EndTime,
        SlotDurationMinutes = ws.SlotDurationMinutes,
        MaxSlots = ws.MaxSlots,
        IsActive = ws.IsActive
    };
}