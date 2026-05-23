using ClinicManagement.Models;
using ClinicManagement.Repositories;

namespace ClinicManagement.Services;

public class NotificationService
{
    private readonly NotificationRepository _repo;

    public NotificationService(NotificationRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<Notification>> GetForUserAsync(int userId, string role)
    {
        // combine user-specific and role notifications
        var list = await _repo.GetForUserAsync(userId, 50);
        // also include role-targeted ones
        var roleList = await _repo.GetForRoleAsync(role, 50);
        var combined = list.Concat(roleList)
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .ToList();
        return combined;
    }

    public async Task<int> CountUnreadAsync(int userId, string role)
    {
        return await _repo.CountUnreadAsync(userId, role);
    }

    public async Task<Notification> CreateAsync(Notification n) => await _repo.CreateAsync(n);

    public async Task MarkAsReadAsync(int id) => await _repo.MarkAsReadAsync(id);
}
