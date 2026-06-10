using ClinicManagement.Data;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repositories;

public class NotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Notification>> GetForUserAsync(int userId, int limit = 20)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(limit)
            .ToListAsync();//thực thi câu truy vấn bất đồng bộ 
    }

    public async Task<List<Notification>> GetForRoleAsync(string role, int limit = 20)
    {
        return await _context.Notifications
            .Where(n => (n.Role == role) || (n.UserId == null && n.Role == role))
            .OrderByDescending(n => n.CreatedAt)
            .Take(limit)
            .ToListAsync();//thực thi câu truy vấn bất đồng bộ 
    }

    public async Task<int> CountUnreadAsync(int userId, string role)
    {
        return await _context.Notifications
            .Where(n => !n.IsRead && (n.UserId == userId || n.Role == role))
            .CountAsync();
    }

    public async Task<Notification> CreateAsync(Notification n)
    {
        _context.Notifications.Add(n);
        await _context.SaveChangesAsync();
        return n;
    }

    public async Task MarkAsReadAsync(int id)
    {
        var n = await _context.Notifications.FindAsync(id);
        if (n == null) return;
        n.IsRead = true;
        _context.Notifications.Update(n);
        await _context.SaveChangesAsync();
    }
}
