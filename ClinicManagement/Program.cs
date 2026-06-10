using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using ClinicManagement.Data;
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

// Cookie Authentication  
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; //Chưa login thì redirect về đây
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied"; //K đủ quyền, sai role thì redirect về đây
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true; // Tự gia hạn nếu còn dùng
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

// notifications
builder.Services.AddScoped<NotificationRepository>();
builder.Services.AddScoped<NotificationService>();

//Vnpay
var vnpayConfig = builder.Configuration.GetSection("VNPAY");
builder.Services.AddVnpayClient(config =>
{
    config.TmnCode = vnpayConfig["TmnCode"]!;
    config.HashSecret = vnpayConfig["HashSecret"]!;
    config.CallbackUrl = vnpayConfig["CallbackUrl"]!;
});

// THAY BẰNG
//email
builder.Services.AddHttpClient();
builder.Services.AddScoped<EmailService>();

//payment repository để check xem lịch hẹn đã có hóa đơn chưa khi tạo hồ sơ bệnh án
builder.Services.AddScoped<PaymentRepository>();

// cloudinary
builder.Services.AddSingleton<CloudinaryService>();

var app = builder.Build();

// Tạo thư mục uploads nếu chưa có
var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads", "avatars");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);



// Seed admin
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    context.Database.ExecuteSqlRaw(@"
        CREATE TABLE IF NOT EXISTS `Notifications` (
            `Id` int NOT NULL AUTO_INCREMENT,
            `UserId` int NULL,
            `Role` longtext NULL,
            `Title` longtext NOT NULL,
            `Message` longtext NOT NULL,
            `IsRead` tinyint(1) NOT NULL,
            `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
            `Link` longtext NULL,
            PRIMARY KEY (`Id`),
            KEY `IX_Notifications_UserId_IsRead` (`UserId`, `IsRead`),
            CONSTRAINT `FK_Notifications_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
    ");

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

app.UseHttpsRedirection(); //
app.UseStaticFiles();  // Cho phép dùng wwwroot (CSS, JS, ảnh)
app.UseRouting(); //xác định request đi đến controller nào


app.UseAuthentication(); //đọc Cookie trong request (từ trình duyệt), xác định user là ai
//giải mã ra Claims (userId, role, email,...)ASP.NET core sẽ tự động
//gắn vào User object để dùng trong controller

app.UseAuthorization(); //đọc User.Role từ Claims đã giải mã sẵn trong User object đó 
// để kiểm tra role, nếu hợp lệ thì cho vào, ...


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();