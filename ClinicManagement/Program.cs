using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using ClinicManagement.Data;
using ClinicManagement.Middlewares;
using ClinicManagement.Repositories;
using ClinicManagement.Services;
using VNPAY.Extensions;

var builder = WebApplication.CreateBuilder(args);

// MVC thay vì AddControllers
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Cookie Auth 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";         // Chưa login → redirect về đây
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Không đủ quyền → redirect về đây
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;          // Tự gia hạn nếu còn dùng
    })
    
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
        options.CallbackPath = "/signin-google";
    });

builder.Services.AddAuthorization();


// auth
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<AuthService>();

// specialty
builder.Services.AddScoped<SpecialtyRepository>();
builder.Services.AddScoped<SpecialtyService>();

// doctor
builder.Services.AddScoped<DoctorRepository>();
builder.Services.AddScoped<DoctorService>();

// patient
builder.Services.AddScoped<PatientRepository>();
builder.Services.AddScoped<PatientService>();

// working schedule
builder.Services.AddScoped<WorkingScheduleRepository>();
builder.Services.AddScoped<AppointmentSlotRepository>();
builder.Services.AddScoped<WorkingScheduleService>();

// appointment
builder.Services.AddScoped<AppointmentRepository>();
builder.Services.AddScoped<AppointmentService>();

// medical record
builder.Services.AddScoped<MedicalRecordRepository>();
builder.Services.AddScoped<MedicalRecordService>();

// medication
builder.Services.AddScoped<MedicationRepository>();
builder.Services.AddScoped<MedicationService>();

//review
builder.Services.AddScoped<ReviewService>();

//payment
builder.Services.AddScoped<PaymentService>();

//Vnpay
var vnpayConfig = builder.Configuration.GetSection("VNPAY");
builder.Services.AddVnpayClient(config =>
{
    config.TmnCode = vnpayConfig["TmnCode"]!;
    config.HashSecret = vnpayConfig["HashSecret"]!;
    config.CallbackUrl = vnpayConfig["CallbackUrl"]!;
});

//email
builder.Services.AddScoped<EmailService>();

var app = builder.Build();

// Tạo thư mục uploads nếu chưa có
var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads", "avatars");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);

app.UseMiddleware<ExceptionMiddleware>();

// Seed admin
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!context.Users.Any(u => u.Email == "admin@gmail.com"))
    {
        context.Users.Add(new ClinicManagement.Models.User
        {
            FullName = "Admin",
            Email = "admin@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Phone = "0987654321",
            Role = "Admin",
            IsActive = true
        });
        await context.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // Cho phép dùng wwwroot (CSS, JS, ảnh)
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Route mặc định của MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();