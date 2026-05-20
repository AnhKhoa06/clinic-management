using ClinicManagement.DTOs.Doctor;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace ClinicManagement.Controllers;

public class DoctorController : Controller
{
    private readonly DoctorService _service;
    private readonly SpecialtyService _specialtyService;

    public DoctorController(DoctorService service, SpecialtyService specialtyService)
    {
        _service = service;
        _specialtyService = specialtyService;
    }

    // GET /Doctor
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var result = await _service.GetAllAsync();
        return View(result);
    }

    // GET /Doctor/Create
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Specialties = await _specialtyService.GetAllAsync();
        return View();
    }

    // POST /Doctor/Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(DoctorCreateWithAccountDto dto, IFormFile? avatar)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Specialties = await _specialtyService.GetAllAsync();
            return View(dto);
        }

        var (success, message, created) = await _service.CreateWithAccountAsync(dto);
        if (!success)
        {
            ModelState.AddModelError("", message);
            ViewBag.Specialties = await _specialtyService.GetAllAsync();
            return View(dto);
        }

        // Upload avatar nếu có
        if (avatar != null && avatar.Length > 0 && created != null)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (allowedTypes.Contains(avatar.ContentType))
            {
                var ext = Path.GetExtension(avatar.FileName);
                var fileName = $"doctor-{created.Id}-{DateTime.Now:yyyyMMddHHmmss}{ext}";
                var filePath = Path.Combine("wwwroot", "uploads", "avatars", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await avatar.CopyToAsync(stream);

                var updateDto = new DoctorUpdateDto
                {
                    SpecialtyId = created.SpecialtyId,
                    LicenseNumber = created.LicenseNumber,
                    Bio = created.Bio,
                    AvatarUrl = $"/uploads/avatars/{fileName}"
                };
                await _service.UpdateAsync(created.Id, updateDto);
            }
        }

        TempData["Success"] = message;
        return RedirectToAction(nameof(Index));
    }

    // GET /Doctor/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var (success, _, data) = await _service.GetByIdAsync(id);
        if (!success) return NotFound();

        ViewBag.Specialties = await _specialtyService.GetAllAsync();
        ViewBag.Id = id;
        ViewBag.AvatarUrl = data!.AvatarUrl;      
        ViewBag.DoctorName = data!.FullName; 

        var dto = new DoctorUpdateDto
        {
            SpecialtyId = data!.SpecialtyId,
            LicenseNumber = data.LicenseNumber,
            Bio = data.Bio,
            AvatarUrl = data.AvatarUrl
        };

        return View(dto);
    }

    // POST /Doctor/Edit/5
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, DoctorUpdateDto dto, IFormFile? avatar)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Specialties = await _specialtyService.GetAllAsync();
            ViewBag.Id = id;
            var (_, _, d) = await _service.GetByIdAsync(id);
            ViewBag.AvatarUrl = d?.AvatarUrl;
            ViewBag.DoctorName = d?.FullName;
            return View(dto);
        }

        // Xử lý upload ảnh nếu có
        if (avatar != null && avatar.Length > 0)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(avatar.ContentType))
            {
                ModelState.AddModelError("", "Chỉ chấp nhận ảnh JPG, PNG, WEBP.");
                ViewBag.Specialties = await _specialtyService.GetAllAsync();
                ViewBag.Id = id;
                return View(dto);
            }

            var ext = Path.GetExtension(avatar.FileName);
            var fileName = $"doctor-{id}-{DateTime.Now:yyyyMMddHHmmss}{ext}";
            var filePath = Path.Combine("wwwroot", "uploads", "avatars", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await avatar.CopyToAsync(stream);

            dto.AvatarUrl = $"/uploads/avatars/{fileName}";
        }

        var (success, message, _) = await _service.UpdateAsync(id, dto);
        if (!success)
        {
            ModelState.AddModelError("", message);
            ViewBag.Specialties = await _specialtyService.GetAllAsync();
            ViewBag.Id = id;
            return View(dto);
        }

        TempData["Success"] = message;
        return RedirectToAction(nameof(Index));
    }

    // POST /Doctor/ToggleActive/5
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var (success, message) = await _service.ToggleActiveAsync(id);
        if (!success)
            TempData["Error"] = message;
        else
            TempData["Success"] = message;

        return RedirectToAction(nameof(Index));
    }

    // GET /Doctor/Browse — Bệnh nhân xem danh sách theo chuyên khoa
    [Authorize]
    public async Task<IActionResult> Browse(int? specialtyId)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);// Lấy role của user đang đăng nhập từ Cookie
        var name = User.FindFirstValue(ClaimTypes.Name);// Lấy tên của user đang đăng nhập từ Cookie

        var specialties = await _specialtyService.GetAllAsync();//Lấy danh sách chuyên khoa để hiện filter
        
        ViewBag.Specialties = specialties;// bỏ danh sách chuyên khoa vào
        ViewBag.SelectedSpecialtyId = specialtyId;// bỏ id chuyên khoa đang chọn vào

        List<DoctorResponseDto> doctors;
        
        if (specialtyId.HasValue)
            doctors = await _service.GetBySpecialtyAsync(specialtyId.Value);
        else
            doctors = await _service.GetAllAsync();//Lấy tất cả bác sĩ IsActive = true

        doctors = doctors.Where(d => d.IsActive).ToList();
        return View(doctors);
    }

    // GET /Doctor/PublicProfile/5 — Bệnh nhân xem chi tiết bác sĩ
    [Authorize]
    public async Task<IActionResult> PublicProfile(int id)
    {
        var (success, _, data) = await _service.GetByIdAsync(id);
        if (!success) return NotFound();
        return View(data);
    }

    // POST /Doctor/UploadAvatar
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> UploadAvatar(int id, IFormFile avatar)
    {
        if (avatar == null || avatar.Length == 0)
        {
            TempData["Error"] = "Vui lòng chọn ảnh.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(avatar.ContentType))
        {
            TempData["Error"] = "Chỉ chấp nhận ảnh JPG, PNG, WEBP.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        if (avatar.Length > 2 * 1024 * 1024)
        {
            TempData["Error"] = "Ảnh không được vượt quá 2MB.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        var ext = Path.GetExtension(avatar.FileName);
        var fileName = $"doctor-{id}-{DateTime.Now:yyyyMMddHHmmss}{ext}";
        var filePath = Path.Combine("wwwroot", "uploads", "avatars", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
            await avatar.CopyToAsync(stream);

        var avatarUrl = $"/uploads/avatars/{fileName}";

        // Cập nhật AvatarUrl vào DB
        var (success, _, data) = await _service.GetByIdAsync(id);
        if (success && data != null)
        {
            var dto = new DoctorUpdateDto
            {
                SpecialtyId = data.SpecialtyId,
                LicenseNumber = data.LicenseNumber,
                Bio = data.Bio,
                AvatarUrl = avatarUrl
            };
            await _service.UpdateAsync(id, dto);
        }

        TempData["Success"] = "Cập nhật ảnh đại diện thành công.";
        return RedirectToAction(nameof(Edit), new { id });
    }
}