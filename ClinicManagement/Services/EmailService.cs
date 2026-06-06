using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ClinicManagement.Services;

public class EmailService
{
    private readonly IConfiguration _config;
    private readonly HttpClient _http;

    public EmailService(IConfiguration config, IHttpClientFactory httpFactory)
    {
        _config = config;
        _http = httpFactory.CreateClient();
    }

    public async Task SendPaymentSuccessAsync(
        string toEmail, string patientName, string invoiceCode,
        string doctorName, string slotDate, decimal amount, string method)
    {
        try
        {
            var apiKey = _config["Brevo:ApiKey"] ?? _config["Brevo__ApiKey"] ?? "";
            Console.WriteLine($"[EmailService] Gửi tới {toEmail}, key length: {apiKey.Length}");

            var payload = new
            {
                sender = new { email = "anhkhoale2406@gmail.com", name = "Phòng Khám" },
                to = new[] { new { email = toEmail, name = patientName } },
                subject = $"Xác nhận thanh toán thành công – {invoiceCode}",
                htmlContent = BuildEmailHtml(patientName, invoiceCode, doctorName, slotDate, amount, method)
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
            request.Headers.Add("api-key", apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _http.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[EmailService] Status: {response.StatusCode}, Body: {body}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EmailService] Lỗi: {ex.Message}");
        }
    }

    private static string BuildEmailHtml(string patientName, string invoiceCode, string doctorName, string slotDate, decimal amount, string method)
    {
        return $"""
        <!DOCTYPE html>
        <html lang="vi">
        <head><meta charset="UTF-8"></head>
        <body style="margin:0;padding:0;background:#f0f4f8;font-family:'Segoe UI',Arial,sans-serif;">
        <table width="100%" cellpadding="0" cellspacing="0" style="background:#f0f4f8;padding:40px 0;">
            <tr><td align="center">
            <table width="560" cellpadding="0" cellspacing="0" style="background:#fff;border-radius:16px;overflow:hidden;box-shadow:0 4px 24px rgba(0,0,0,0.08);">
                <tr>
                <td style="background:linear-gradient(135deg,#1d4ed8,#2563eb);padding:36px 40px;">
                    <p style="margin:0;font-size:13px;color:#93c5fd;letter-spacing:2px;text-transform:uppercase;">Phòng Khám</p>
                    <h1 style="margin:8px 0 0;font-size:24px;font-weight:700;color:#fff;">Xác nhận thanh toán</h1>
                </td>
                </tr>
                <tr>
                <td style="padding:28px 40px 0;">
                    <p style="margin:0;font-size:15px;color:#374151;">
                    Xin chào <strong>{patientName}</strong>, hóa đơn của bạn đã được thanh toán thành công.
                    </p>
                </td>
                </tr>
                <tr><td style="padding:24px 40px 0;"><hr style="border:none;border-top:1px solid #e5e7eb;" /></td></tr>
                <tr>
                <td style="padding:24px 40px 0;">
                    <table width="100%" cellpadding="0" cellspacing="0" style="font-size:14px;">
                    <tr>
                        <td style="color:#6b7280;padding:10px 0;border-bottom:1px solid #f3f4f6;width:50%;">Mã hóa đơn</td>
                        <td style="font-weight:600;color:#111827;text-align:right;padding:10px 0;border-bottom:1px solid #f3f4f6;">{invoiceCode}</td>
                    </tr>
                    <tr>
                        <td style="color:#6b7280;padding:10px 0;border-bottom:1px solid #f3f4f6;">Bác sĩ</td>
                        <td style="font-weight:600;color:#111827;text-align:right;padding:10px 0;border-bottom:1px solid #f3f4f6;">{doctorName}</td>
                    </tr>
                    <tr>
                        <td style="color:#6b7280;padding:10px 0;border-bottom:1px solid #f3f4f6;">Ngày khám</td>
                        <td style="font-weight:600;color:#111827;text-align:right;padding:10px 0;border-bottom:1px solid #f3f4f6;">{slotDate}</td>
                    </tr>
                    <tr>
                        <td style="color:#6b7280;padding:10px 0;">Phương thức</td>
                        <td style="font-weight:600;color:#111827;text-align:right;padding:10px 0;">{method}</td>
                    </tr>
                    </table>
                </td>
                </tr>
                <tr>
                <td style="padding:24px 40px;">
                    <table width="100%" cellpadding="0" cellspacing="0" style="background:#eff6ff;border-radius:12px;padding:20px;">
                    <tr>
                        <td style="font-size:14px;color:#1d4ed8;font-weight:500;">Tổng thanh toán</td>
                        <td style="font-size:24px;font-weight:800;color:#1d4ed8;text-align:right;">{amount.ToString("N0")} ₫</td>
                    </tr>
                    </table>
                </td>
                </tr>
                <tr>
                <td style="background:#f9fafb;padding:24px 40px;border-top:1px solid #e5e7eb;">
                    <p style="margin:0;font-size:13px;color:#6b7280;">Email này được gửi tự động từ hệ thống phòng khám.</p>
                    <p style="margin:16px 0 0;font-size:12px;color:#9ca3af;">© 2026 Phòng Khám. All rights reserved.</p>
                </td>
                </tr>
            </table>
            </td></tr>
        </table>
        </body>
        </html>
        """;
    }
}