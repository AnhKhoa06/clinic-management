using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace ClinicManagement.Controllers;

public class HomeController : Controller
{
    private readonly DoctorService _doctorService;
    private readonly PatientService _patientService;
    private readonly AppointmentService _appointmentService;
    private readonly MedicationService _medicationService;
    private readonly PaymentService _paymentService;
    private readonly MedicalRecordService _medicalRecordService;

    public HomeController(
        DoctorService doctorService,
        PatientService patientService,
        AppointmentService appointmentService,
        MedicationService medicationService,
        PaymentService paymentService,
        MedicalRecordService medicalRecordService)
    {
        _doctorService = doctorService;
        _patientService = patientService;
        _appointmentService = appointmentService;
        _medicationService = medicationService;
        _paymentService = paymentService;
        _medicalRecordService = medicalRecordService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Admin"))
        {
            var doctors      = await _doctorService.GetAllAsync();
            var patients     = await _patientService.GetAllAsync();
            var appointments = await _appointmentService.GetAllAsync();
            var payments     = await _paymentService.GetAllAsync();

            // 4 stat cards
            ViewBag.DoctorCount      = doctors.Count;
            ViewBag.PatientCount     = patients.Count;
            ViewBag.AppointmentToday = appointments
                .Count(a => a.SlotDate == DateOnly.FromDateTime(DateTime.Today)
                        && a.Status != "Cancelled");
            ViewBag.RevenueThisMonth = payments
                .Where(p => p.Status == "Paid" && p.PaidAt.HasValue
                        && p.PaidAt.Value.Month == DateTime.Today.Month
                        && p.PaidAt.Value.Year == DateTime.Today.Year)
                .Sum(p => p.Amount);

            // Biểu đồ 7 ngày
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateOnly.FromDateTime(DateTime.Today.AddDays(-6 + i)))
                .ToList();

            ViewBag.ChartLabels = System.Text.Json.JsonSerializer.Serialize(
                last7Days.Select(d => d.DayOfWeek switch
                {
                    DayOfWeek.Monday    => "T2",
                    DayOfWeek.Tuesday   => "T3",
                    DayOfWeek.Wednesday => "T4",
                    DayOfWeek.Thursday  => "T5",
                    DayOfWeek.Friday    => "T6",
                    DayOfWeek.Saturday  => "T7",
                    _                   => "CN"
                }).ToList()
            );

            ViewBag.ChartTotal = System.Text.Json.JsonSerializer.Serialize(
                last7Days.Select(d => appointments
                    .Count(a => a.SlotDate == d && a.Status != "Cancelled"))
                .ToList()
            );

            ViewBag.ChartCompleted = System.Text.Json.JsonSerializer.Serialize(
                last7Days.Select(d => appointments
                    .Count(a => a.SlotDate == d && a.Status == "Completed"))
                .ToList()
            );

            //tính tổng lịch hẹn
            ViewBag.ChartTotalSum = last7Days
                .Sum(d => appointments.Count(a => a.SlotDate == d && a.Status != "Cancelled"));

            //tính đã hoàn thành
            ViewBag.ChartCompletedSum = last7Days
                .Sum(d => appointments.Count(a => a.SlotDate == d && a.Status == "Completed"));

            // Lịch hẹn hôm nay
            ViewBag.TodayAppointments = appointments
                .Where(a => a.SlotDate == DateOnly.FromDateTime(DateTime.Today)
                        && a.Status != "Cancelled")
                .OrderBy(a => a.SlotTime)
                .Take(5)
                .ToList();

            // Top bác sĩ
            ViewBag.TopDoctors = doctors
                .Where(d => d.AverageRating > 0)
                .OrderByDescending(d => d.AverageRating)
                .Take(3)
                .ToList();
        }

        if (User.IsInRole("Doctor"))
        {
            var userId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!);
            var doctor = await _doctorService.GetByUserIdAsync(userId);

            if (doctor != null)
            {
                var appointments = await _appointmentService.GetByDoctorIdAsync(doctor.Id);
                var medicalRecords = await _medicalRecordService.GetByDoctorIdAsync(doctor.Id);

                // Stat cards
                ViewBag.DoctorAppointmentToday = appointments
                    .Count(a => a.SlotDate == DateOnly.FromDateTime(DateTime.Today)
                            && a.Status != "Cancelled");
                ViewBag.DoctorAppointmentPending = appointments
                    .Count(a => a.Status == "Pending");
                ViewBag.DoctorMedicalRecordCount = medicalRecords.Count;
                ViewBag.DoctorAverageRating = doctor.AverageRating;

                // Lịch hẹn hôm nay chi tiết
                ViewBag.DoctorTodayAppointments = appointments
                    .Where(a => a.SlotDate == DateOnly.FromDateTime(DateTime.Today)
                            && a.Status != "Cancelled")
                    .OrderBy(a => a.SlotTime)
                    .Take(5)
                    .ToList();

                // Lịch hẹn sắp tới trong tuần
                var nextWeek = DateOnly.FromDateTime(DateTime.Today.AddDays(7));
                ViewBag.DoctorUpcomingAppointments = appointments
                    .Where(a => a.SlotDate > DateOnly.FromDateTime(DateTime.Today)
                            && a.SlotDate <= nextWeek
                            && a.Status != "Cancelled")
                    .OrderBy(a => a.SlotDate).ThenBy(a => a.SlotTime)
                    .Take(5)
                    .ToList();
            }
        }

        if (User.IsInRole("Patient"))
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (_, _, patient) = await _patientService.GetByUserIdAsync(userId);

            if (patient != null)
            {
                var appointments = await _appointmentService.GetByPatientIdAsync(patient.Id);
                var payments = await _paymentService.GetByPatientAsync(userId);

                // Stat cards
                ViewBag.PatientAppointmentTotal = appointments.Count;
                ViewBag.PatientAppointmentPending = appointments
                    .Count(a => a.Status == "Pending" || a.Status == "Confirmed");
                ViewBag.PatientAppointmentCompleted = appointments
                    .Count(a => a.Status == "Completed");
                ViewBag.PatientUnpaidCount = payments
                    .Count(p => p.Status == "Unpaid");

                // Lịch hẹn sắp tới (Pending/Confirmed, ngày >= hôm nay, lấy 3 cái gần nhất)
                ViewBag.UpcomingAppointments = appointments
                    .Where(a => (a.Status == "Pending" || a.Status == "Confirmed")
                            && a.SlotDate >= DateOnly.FromDateTime(DateTime.Today))
                    .OrderBy(a => a.SlotDate).ThenBy(a => a.SlotTime)
                    .Take(3)
                    .ToList();

                // Hóa đơn chưa thanh toán
                ViewBag.UnpaidPayments = payments
                    .Where(p => p.Status == "Unpaid")
                    .OrderByDescending(p => p.Id)
                    .Take(3)
                    .ToList();

                // Lịch tái khám sắp tới (từ MedicalRecord)
                var medicalRecords = await _medicalRecordService.GetByPatientIdAsync(patient.Id);
                ViewBag.UpcomingFollowUps = medicalRecords
                    .Where(mr => mr.FollowUpDate.HasValue
                            && mr.FollowUpDate.Value >= DateOnly.FromDateTime(DateTime.Today))
                    .OrderBy(mr => mr.FollowUpDate)
                    .Take(3)
                    .ToList();
            }
        }

        

        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}