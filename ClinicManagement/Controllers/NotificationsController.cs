using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicManagement.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _service;

    public NotificationsController(NotificationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return Unauthorized();

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var role = User.IsInRole("Admin") ? "Admin" : User.IsInRole("Doctor") ? "Doctor" : "Patient";

        var items = await _service.GetForUserAsync(userId, role);

        var dto = items.Select(n => new {
            id = n.Id,
            title = n.Title,
            message = n.Message,
            isRead = n.IsRead,
            createdAt = n.CreatedAt,
            link = n.Link
        });

        return Ok(dto);
    }

    [HttpPost("mark-read/{id}")]
    public async Task<IActionResult> MarkRead(int id)
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return Unauthorized();

        await _service.MarkAsReadAsync(id);
        return Ok();
    }
}
