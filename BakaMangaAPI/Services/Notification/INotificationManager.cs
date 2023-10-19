using BakaMangaAPI.DTOs;

namespace BakaMangaAPI.Services.Notification;

public interface INotificationManager
{
    Task<NotificationDTO> CreateNotificationAsync();
    Task SendToUserAsync(string userId, NotificationDTO notification);
}
