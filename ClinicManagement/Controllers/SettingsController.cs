using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

[Authorize]
public class SettingsController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Cài đặt";
        return View();
    }
}