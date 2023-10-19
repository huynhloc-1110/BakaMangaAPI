using BakaMangaAPI.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace BakaMangaAPI.Services.Notification;

public class NotificationManager : INotificationManager
{
    private readonly IHubContext<NotificationHub> _notificationHubContext;
    private readonly IUserConnectionManager _userConnectionManager;

    public NotificationManager(IHubContext<NotificationHub> notificationHubContext,
        IUserConnectionManager userConnectionManager)
    {
        _notificationHubContext = notificationHubContext;
        _userConnectionManager = userConnectionManager;
    }

    public Task<NotificationDTO> CreateNotificationAsync()
    {
        throw new NotImplementedException();
    }

    public async Task SendToUserAsync(string userId, NotificationDTO notification)
    {
        var connections = _userConnectionManager.GetUserConnections(userId);
        if (connections != null && connections.Count > 0)
        {
            foreach (var connectionId in connections)
            {
                await _notificationHubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveNotification", notification);
            }
        }
    }
}
