using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

public class HomeController : Controller
{
    private readonly DoctorService _doctorService;
    private readonly PatientService _patientService;
    private readonly AppointmentService _appointmentService;
    private readonly MedicationService _medicationService;

    public HomeController(
        DoctorService doctorService,
        PatientService patientService,
        AppointmentService appointmentService,
        MedicationService medicationService)
    {
        _doctorService = doctorService;
        _patientService = patientService;
        _appointmentService = appointmentService;
        _medicationService = medicationService;
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

            ViewBag.DoctorCount      = doctors.Count;
            ViewBag.PatientCount     = patients.Count;
            ViewBag.AppointmentToday = appointments
                .Count(a => a.SlotDate == DateOnly.FromDateTime(DateTime.Today)
                        && a.Status != "Cancelled");
            ViewBag.MedicationCount  = medications.Count;
        }

        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}