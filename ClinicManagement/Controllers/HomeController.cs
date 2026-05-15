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
            var medications  = await _medicationService.GetAllAsync();
            var payments     = await _paymentService.GetAllAsync();

            ViewBag.DoctorCount      = doctors.Count;
            ViewBag.PatientCount     = patients.Count;
            ViewBag.AppointmentToday = appointments
                .Count(a => a.SlotDate == DateOnly.FromDateTime(DateTime.Today)
                        && a.Status != "Cancelled");
            ViewBag.MedicationCount  = medications.Count;
            ViewBag.AppointmentPending   = appointments.Count(a => a.Status == "Pending");
            ViewBag.AppointmentCompleted = appointments.Count(a => a.Status == "Completed");

            // Doanh thu
            ViewBag.RevenueToday  = payments
                .Where(p => p.Status == "Paid" && p.PaidAt.HasValue
                        && p.PaidAt.Value.Date == DateTime.Today)
                .Sum(p => p.Amount);
            ViewBag.RevenueTotal  = payments
                .Where(p => p.Status == "Paid")
                .Sum(p => p.Amount);
            ViewBag.RevenueUnpaid = payments
                .Where(p => p.Status == "Unpaid")
                .Sum(p => p.Amount);

            // Top bác sĩ
            ViewBag.TopDoctors = doctors
                .Where(d => d.AverageRating > 0)
                .OrderByDescending(d => d.AverageRating)
                .Take(3)
                .ToList();

            // Lịch hẹn hôm nay chi tiết
            ViewBag.TodayAppointments = appointments
                .Where(a => a.SlotDate == DateOnly.FromDateTime(DateTime.Today)
                        && a.Status != "Cancelled")
                .OrderBy(a => a.SlotTime)
                .Take(5)
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