# Clinic Management

Hệ thống quản lý phòng khám xây dựng bằng ASP.NET Core MVC + MySQL.

## Yêu cầu cài đặt trước

- [MySQL Server 8.0](https://dev.mysql.com/downloads/mysql/)
- [MySQL Workbench](https://dev.mysql.com/downloads/workbench/)
- [Git](https://git-scm.com/downloads)

## Cách chạy project

### Bước 1 — Clone repo về

- Mở terminal (PowerShell hoặc Git Bash) và chạy:

* cd C:\Users\TÊN_MÁY_BẠN\Documents
* git clone https://github.com/AnhKhoa06/clinic-management.git
* cd clinic-management/ClinicManagement

### Bước 2 — Tạo file cấu hình

- Đổi tên file appsettings.Example.json thành appsettings.json.
- Sau đó mở file file `appsettings.json`, sửa dòng Password thành mật khẩu MySQL của bạn:

```json
"DefaultConnection": "Server=localhost;Port=3306;Database=clinic_management;User=root;Password=MẬT_KHẨU_CỦA_BẠN;"
```

### Bước 3 — Import database

1. Mở **MySQL Workbench**
2. Vào **Server → Data Import**
3. Chọn **Import from Self-Contained File**
4. Click Browse → chọn file `clinic_management.sql` trong thư mục gốc repo
5. Ở phần **Default Schema to be Imported To** → gõ `clinic_management`
6. Click **Start Import**

### Bước 4 — Set Google OAuth (xin Client ID và Secret từ bạn Khoa)

- dotnet user-secrets init
- dotnet user-secrets set "Authentication:Google:ClientId" "XIN_TỪ_BẠN_KHOA"
- dotnet user-secrets set "Authentication:Google:ClientSecret" "XIN_TỪ_BẠN_KHOA"

### Bước 5 — Chạy project

- dotnet restore
- dotnet run
- Mở trình duyệt vào `http://localhost:5194`

## Tài khoản mặc định

- **Admin:** `admin@gmail.com` / `admin123`
- **Bác Sỹ:** `doctor@gmail.com` / `123456`
