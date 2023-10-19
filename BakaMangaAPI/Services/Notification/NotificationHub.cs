using Microsoft.AspNetCore.SignalR;

namespace BakaMangaAPI.Services.Notification;

public class NotificationHub : Hub
{
    private readonly IUserConnectionManager _userConnectionManager;

    public NotificationHub(IUserConnectionManager userConnectionManager)
    {
        _userConnectionManager = userConnectionManager;
    }

    public string RegisterConnection(string userId)
    {
        _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);
        return Context.ConnectionId;
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        // Unregister connection
        var connectionId = Context.ConnectionId;
        _userConnectionManager.RemoveUserConnection(connectionId);

        await base.OnDisconnectedAsync(exception);
    }
}
