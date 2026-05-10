using System.Security.Claims;
using ClinicManagement.DTOs.Auth;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    // GET /Auth/Login
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View();
    }

    // GET /Auth/LoginWithGoogle
    [HttpGet]
    public IActionResult LoginWithGoogle()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleCallback", "Auth")
        };
        return Challenge(properties, "Google");
    }

    // GET /Auth/GoogleCallback
    [HttpGet]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync("Google");
        if (!result.Succeeded)
        {
            TempData["Error"] = "Đăng nhập Google thất bại.";
            return RedirectToAction("Login");
        }

        var email    = result.Principal.FindFirstValue(ClaimTypes.Email)!;
        var fullName = result.Principal.FindFirstValue(ClaimTypes.Name)!;

        var (success, message, user) = await _authService.LoginWithGoogleAsync(email, fullName);
        if (!success)
        {
            TempData["Error"] = message;
            return RedirectToAction("Login");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user!.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role),
        };

        var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = true });

        return RedirectToAction("Index", "Home");
    }

    // POST /Auth/Login
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var (success, message, user) = await _authService.LoginAsync(dto);
        if (!success)
        {
            ModelState.AddModelError("", message);
            return View(dto);
        }

        // Tạo claims từ thông tin user
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user!.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = true }
        );

        // Redirect theo role
        return user.Role switch
        {
            "Admin" => RedirectToAction("Index", "Home"),
            "Doctor" => RedirectToAction("Index", "Home"),
            "Patient" => RedirectToAction("Index", "Home"),
            _ => RedirectToAction("Index", "Home")
        };
    }

    // GET /Auth/Register
    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View();
    }

    // POST /Auth/Register
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var (success, message) = await _authService.RegisterAsync(dto);
        if (!success)
        {
            ModelState.AddModelError("", message);
            return View(dto);
        }

        TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
        return RedirectToAction("Login");
    }

    // POST /Auth/Logout
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    // GET /Auth/AccessDenied
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    // GET /Auth/ChangePassword
    [Authorize]
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    // POST /Auth/ChangePassword
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (success, message) = await _authService.ChangePasswordAsync(userId, dto);

        if (!success)
        {
            ModelState.AddModelError("", message);
            return View(dto);
        }

        TempData["Success"] = message;
        return RedirectToAction("ChangePassword");
    }
}