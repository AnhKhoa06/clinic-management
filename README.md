# Clinic Management

Hệ thống quản lý phòng khám xây dựng bằng ASP.NET Core MVC + MySQL.

## Cách chạy project

1. Clone repo về
2. Copy file `ClinicManagement/appsettings.Example.json` → đổi tên thành `appsettings.json`
3. Sửa `Password=YOUR_MYSQL_PASSWORD` thành mật khẩu MySQL của bạn
4. Import database: Mở MySQL Workbench → Server → Data Import → chọn file `clinic_management.sql`
5. Set Google OAuth secrets:
   cd ClinicManagement
   dotnet user-secrets init
   dotnet user-secrets set "Authentication:Google:ClientId" "XIN_CLIENT_ID_từ bạn Khoa"
   dotnet user-secrets set "Authentication:Google:ClientSecret" "XIN_CLIENT_SECRET_từ bạn Khoa"
6. Chạy project:
   cd ClinicManagement
   dotnet restore
   dotnet run

## Tài khoản mặc định

- Admin: `admin@gmail.com` / `admin123`
