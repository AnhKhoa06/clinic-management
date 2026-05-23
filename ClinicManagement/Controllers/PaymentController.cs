using System.Security.Claims;
using ClinicManagement.DTOs.Payment;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNPAY;
using VNPAY.Models;
using VNPAY.Models.Enums;
using VNPAY.Models.Exceptions;

namespace ClinicManagement.Controllers;

public class PaymentController : Controller
{
    private readonly PaymentService _paymentService;
    private readonly IVnpayClient _vnpayClient;
    private readonly DoctorService _doctorService;
    private readonly AppointmentService _appointmentService;
    private readonly MedicalRecordService _medicalRecordService;

    public PaymentController(PaymentService paymentService, IVnpayClient vnpayClient, DoctorService doctorService,         AppointmentService appointmentService,
        MedicalRecordService medicalRecordService )
    {
        _paymentService = paymentService;
        _vnpayClient = vnpayClient;
        _doctorService = doctorService;
        _appointmentService = appointmentService;
        _medicalRecordService = medicalRecordService;

    }

    // GET /Payment — Admin xem tất cả
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Index()
    {
        List<PaymentResponseDto> list;

        if (User.IsInRole("Doctor"))
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var doctor = await _doctorService.GetByUserIdAsync(userId);
            if (doctor == null) return View(new List<PaymentResponseDto>());
            list = await _paymentService.GetByDoctorIdAsync(doctor.Id);
        }
        else
        {
            list = await _paymentService.GetAllAsync();
        }

        return View(list);
    }

    // POST /Payment/SetMethod — Admin chọn phương thức trước khi xác nhận
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetMethod(int id, string method)
    {
        var (success, message) = await _paymentService.SetMethodAsync(id, method);
        TempData[success ? "Success" : "Error"] = message;
        return RedirectToAction(nameof(Index));
    }

    // POST /Payment/MarkPaid/5 — Admin đánh dấu đã thanh toán
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkPaid(int id)
    {
        var (success, message) = await _paymentService.MarkPaidAsync(id);
        TempData[success ? "Success" : "Error"] = message;
        return RedirectToAction(nameof(Index));
    }

    // GET /Payment/MyPayments — Bệnh nhân xem hóa đơn của mình
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> MyPayments()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var list = await _paymentService.GetByPatientAsync(userId);
        return View(list);
    }

    // GET /Payment/Detail/5
    [Authorize]
    public async Task<IActionResult> Detail(int id)
    {
        var (success, _, data) = await _paymentService.GetByIdAsync(id);
        if (!success) return NotFound();
        return View(data);
    }

    //VNPAY
    // GET /Payment/PayOnline/5
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> PayOnline(int id)
    {
        var (success, _, data) = await _paymentService.GetByIdAsync(id);
        if (!success || data == null) return NotFound();

        if (data.Status == "Paid")
        {
            TempData["Error"] = "Hóa đơn này đã được thanh toán.";
            return RedirectToAction(nameof(MyPayments));
        }

        // Thêm dòng này để debug
        Console.WriteLine($"[VNPAY DEBUG] Amount={data.Amount} | OrderInfo=PAYID:{id}|{data.InvoiceCode}");

        var paymentUrl = _vnpayClient.CreatePaymentUrl(
            (double)Math.Round(data.Amount, 0),
            $"PAYID:{id}|{data.InvoiceCode}",
            BankCode.ANY
        );

        Console.WriteLine($"[VNPAY DEBUG] URL={paymentUrl.Url}");

        return Redirect(paymentUrl.Url);
    }

    // GET /Payment/VnPayReturn
    [AllowAnonymous]
    public async Task<IActionResult> VnPayReturn()
    {
        try
        {
            var result = _vnpayClient.GetPaymentResult(Request);
            var orderInfo = Request.Query["vnp_OrderInfo"].ToString();

            // Parse paymentId từ OrderInfo: "PAYID:2|INV-20260511-0005"
            var paymentId = int.Parse(orderInfo.Split('|')[0].Replace("PAYID:", "").Trim());

            var (success, message) = await _paymentService.MarkPaidVnPayAsync(paymentId);
            var successMsg = Uri.EscapeDataString("Thanh toán thành công!");

            if (success)
                return Redirect($"http://localhost:5194/Payment/MyPayments?toast=success&msg={successMsg}");
            else
                return Redirect($"http://localhost:5194/Payment/MyPayments?toast=error&msg={Uri.EscapeDataString(message)}");
        }
        catch (Exception ex)
        {
            return Redirect($"http://localhost:5194/Payment/MyPayments?toast=error&msg={Uri.EscapeDataString(ex.Message)}");
        }
    }
}