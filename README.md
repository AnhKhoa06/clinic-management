# Clinic Management

Hệ thống quản lý phòng khám xây dựng bằng ASP.NET Core MVC + MySQL.

## Yêu cầu cài đặt trước

- [MySQL Server 8.0](https://dev.mysql.com/downloads/mysql/)
- [MySQL Workbench](https://dev.mysql.com/downloads/workbench/)
- [Git](https://git-scm.com/downloads)
- [ngrok](https://ngrok.com/download)

## Cách chạy project

### Bước 1 — Clone repo về

Mở terminal (PowerShell hoặc Git Bash) và chạy:

```bash
cd C:\Users\TÊN_MÁY_BẠN\Documents
git clone https://github.com/AnhKhoa06/clinic-management.git
cd clinic-management/ClinicManagement
```

### Bước 2 — Tạo file cấu hình

- Đổi tên file `appsettings.Example.json` thành `appsettings.json`.
- Sau đó mở file `appsettings.json`, sửa dòng Password thành mật khẩu MySQL của bạn:

```json
"DefaultConnection": "Server=localhost;Port=3306;Database=clinic_management;User=root;Password=MẬT_KHẨU_CỦA_BẠN;charset=utf8mb4;"
```

### Bước 3 — Import database

1. Mở **MySQL Workbench**
2. Vào **Server → Data Import**
3. Chọn **Import from Self-Contained File**
4. Click **Browse** → chọn file `clinic_management.sql` trong thư mục gốc repo
5. Ở phần **Default Schema to be Imported To** → gõ `clinic_management`
6. Click **Start Import**

### Bước 4 — Set Google OAuth (xin Client ID và Secret từ bạn Khoa)

```bash
dotnet user-secrets init
dotnet user-secrets set "Authentication:Google:ClientId" "XIN_TỪ_BẠN_KHOA"
dotnet user-secrets set "Authentication:Google:ClientSecret" "XIN_TỪ_BẠN_KHOA"
```

### Bước 5 — Set VNPAY (xin TmnCode và HashSecret từ bạn Khoa)

```bash
dotnet user-secrets set "VNPAY:TmnCode" "XIN_TỪ_BẠN_KHOA"
dotnet user-secrets set "VNPAY:HashSecret" "XIN_TỪ_BẠN_KHOA"
```

### Bước 6 — Cài đặt và chạy ngrok

VNPAY cần URL công khai để callback sau khi thanh toán. Dùng ngrok để tạo tunnel từ localhost ra ngoài.

1. Tải và cài [ngrok](https://ngrok.com/download)
2. Đăng ký tài khoản tại [ngrok.com](https://ngrok.com) và lấy authtoken
3. Đăng ký authtoken (chỉ cần làm 1 lần):

```bash
ngrok config add-authtoken TOKEN_CỦA_BẠN
```

4. Chạy ngrok trên port của app:

```bash
ngrok http 5194
```

5. Copy URL dạng `https://xxxx.ngrok-free.app` rồi cập nhật vào `appsettings.json`:

```json
"CallbackUrl": "https://xxxx.ngrok-free.app/Payment/VnPayReturn"
```

> ⚠️ Mỗi lần khởi động lại ngrok, URL sẽ thay đổi — cần cập nhật lại `CallbackUrl`.

### Bước 7 — Chạy project

```bash
dotnet restore
dotnet run
```

Mở trình duyệt vào `http://localhost:5194`

## Tài khoản mặc định

| Vai trò | Email            | Mật khẩu |
| ------- | ---------------- | -------- |
| Admin   | admin@gmail.com  | admin123 |
| Bác sĩ  | doctor@gmail.com | 123456   |
