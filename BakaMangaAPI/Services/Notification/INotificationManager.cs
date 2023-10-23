using BakaMangaAPI.Models;

namespace BakaMangaAPI.Services.Notification;

public interface INotificationManager
{
    Task HandleRequestNotificationAsync(Request request);
    Task HandleChapterNotificationAsync(Chapter chapter);
}
