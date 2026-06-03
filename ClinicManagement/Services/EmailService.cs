using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace ClinicManagement.Services;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendPaymentSuccessAsync(
        string toEmail,
        string patientName,
        string invoiceCode,
        string doctorName,
        string slotDate,
        decimal amount,
        string method)
    {
        try
        {
            var host     = _config["Email:Host"]!;
            var port     = int.Parse(_config["Email:Port"]!);
            var username = _config["Email:Username"]!;
            var password = _config["Email:Password"]!;
            var fromName = _config["Email:FromName"] ?? "Phòng Khám";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, username));
            message.To.Add(new MailboxAddress(patientName, toEmail));
            message.Subject = $"Xác nhận thanh toán thành công – {invoiceCode}";

            var css = @"<style>
                @media only screen and (max-width: 600px) {
                    .wrapper { padding: 16px 0 !important; }
                    .main-table { width: 100% !important; border-radius: 0 !important; }
                    .content-pad { padding: 20px !important; }
                    .header-pad { padding: 24px 20px !important; }
                    .total-amount { font-size: 18px !important; }
                }
            </style>";

            message.Body = new TextPart("html")
            {
                Text = $"""
                <!DOCTYPE html>
                <html lang="vi">
                <head>
                    <meta charset="UTF-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                    {css}
                </head>
                <body style="margin:0; padding:0; background:#f0f4f8; font-family:'Segoe UI',Arial,sans-serif;">
                <table class="wrapper" width="100%" cellpadding="0" cellspacing="0" style="background:#f0f4f8; padding:40px 0;">
                    <tr><td align="center">
                    <table class="main-table" width="560" cellpadding="0" cellspacing="0" style="background:#ffffff; border-radius:16px; overflow:hidden; box-shadow:0 4px 24px rgba(0,0,0,0.08);">
                        
                        <!-- Header -->
                        <tr>
                        <td class="header-pad" style="background:linear-gradient(135deg,#1d4ed8,#2563eb); padding:36px 40px;">
                            <p style="margin:0; font-size:13px; color:#93c5fd; letter-spacing:2px; text-transform:uppercase;">Phòng Khám</p>
                            <h1 style="margin:8px 0 0; font-size:24px; font-weight:700; color:#ffffff;">Xác nhận thanh toán</h1>
                        </td>
                        </tr>

                        <!-- Status badge -->
                        <tr>
                        <td class="content-pad" style="padding:28px 40px 0;">
                            <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="background:#dcfce7; border-radius:20px; padding:6px 16px;">
                                <span style="color:#16a34a; font-size:13px; font-weight:600;">Thanh toán thành công</span>
                                </td>
                            </tr>
                            </table>
                            <p style="margin:16px 0 0; font-size:15px; color:#374151;">
                            Xin chào <strong>{patientName}</strong>, hóa đơn của bạn đã được thanh toán thành công.
                            </p>
                        </td>
                        </tr>

                        <!-- Divider -->
                        <tr><td class="content-pad" style="padding:24px 40px 0;"><hr style="border:none; border-top:1px solid #e5e7eb;" /></td></tr>

                        <!-- Invoice details -->
                        <tr>
                        <td class="content-pad" style="padding:24px 40px 0;">
                            <p style="margin:0 0 16px; font-size:11px; font-weight:700; color:#9ca3af; letter-spacing:1.5px; text-transform:uppercase;">Chi tiết hóa đơn</p>
                            <table width="100%" cellpadding="0" cellspacing="0" style="font-size:14px;">
                            <tr>
                                <td style="color:#6b7280; padding:10px 0; border-bottom:1px solid #f3f4f6; width:50%;">Mã hóa đơn</td>
                                <td style="font-weight:600; color:#111827; text-align:right; padding:10px 0; border-bottom:1px solid #f3f4f6; font-family:monospace; font-size:13px; word-break:break-all;">{invoiceCode}</td>
                            </tr>
                            <tr>
                                <td style="color:#6b7280; padding:10px 0; border-bottom:1px solid #f3f4f6;">Bác sĩ</td>
                                <td style="font-weight:600; color:#111827; text-align:right; padding:10px 0; border-bottom:1px solid #f3f4f6;">{doctorName}</td>
                            </tr>
                            <tr>
                                <td style="color:#6b7280; padding:10px 0; border-bottom:1px solid #f3f4f6;">Ngày khám</td>
                                <td style="font-weight:600; color:#111827; text-align:right; padding:10px 0; border-bottom:1px solid #f3f4f6;">{slotDate}</td>
                            </tr>
                            <tr>
                                <td style="color:#6b7280; padding:10px 0;">Phương thức</td>
                                <td style="font-weight:600; color:#111827; text-align:right; padding:10px 0;">{method}</td>
                            </tr>
                            </table>
                        </td>
                        </tr>

                        <!-- Total -->
                        <tr>
                        <td class="content-pad" style="padding:24px 40px;">
                            <table width="100%" cellpadding="0" cellspacing="0" style="background:#eff6ff; border-radius:12px; padding:20px;">
                            <tr>
                                <td style="font-size:14px; color:#1d4ed8; font-weight:500;">Tổng thanh toán</td>
                                <td class="total-amount" style="font-size:24px; font-weight:800; color:#1d4ed8; text-align:right;">{amount.ToString("N0")} ₫</td>
                            </tr>
                            </table>
                        </td>
                        </tr>

                        <!-- Footer -->
                        <tr>
                        <td class="content-pad" style="background:#f9fafb; padding:24px 40px; border-top:1px solid #e5e7eb;">
                            <p style="margin:0; font-size:13px; color:#6b7280; line-height:1.6;">
                            Email này được gửi tự động từ hệ thống phòng khám.<br/>
                            Nếu bạn có thắc mắc, vui lòng liên hệ trực tiếp với phòng khám để được hỗ trợ.
                            </p>
                            <p style="margin:16px 0 0; font-size:12px; color:#9ca3af;">© 2026 Phòng Khám. All rights reserved.</p>
                        </td>
                        </tr>

                    </table>
                    </td></tr>
                </table>
                </body>
                </html>
                """
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(username, password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EmailService] Gửi mail thất bại: {ex.GetType().Name} | {ex.Message} | {ex.InnerException?.Message}");
            throw; // tạm thời throw để thấy lỗi
        }
    }
}