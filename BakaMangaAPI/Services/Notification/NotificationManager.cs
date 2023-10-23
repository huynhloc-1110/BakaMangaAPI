using AutoMapper;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace BakaMangaAPI.Services.Notification;

public class NotificationManager : INotificationManager
{
    private readonly IHubContext<NotificationHub> _notificationHubContext;
    private readonly IUserConnectionManager _userConnectionManager;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public NotificationManager(IHubContext<NotificationHub> notificationHubContext,
        IUserConnectionManager userConnectionManager,
        ApplicationDbContext context,
        IMapper mapper)
    {
        _notificationHubContext = notificationHubContext;
        _userConnectionManager = userConnectionManager;
        _context = context;
        _mapper = mapper;
    }

    private async Task SendToUserAsync(string userId, NotificationDTO notification)
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

    private async Task SendToManyUsersAsync(string[] userIds, NotificationDTO notification)
    {
        var connections = _userConnectionManager.GetManyUsersConnections(userIds);
        if (connections != null && connections.Count > 0)
        {
            foreach (var connectionId in connections)
            {
                await _notificationHubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveNotification", notification);
            }
        }
    }

    public async Task HandleRequestNotificationAsync(Request request)
    {
        var notification = new RequestNotification
        {
            Request = request,
            User = request.User,
        };
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        var notificationDto = _mapper.Map<RequestNotificationDTO>(notification);
        await SendToUserAsync(request.User.Id, notificationDto);
    }

    public async Task HandleChapterNotificationAsync(Chapter chapter)
    {
        var notifications = new List<ChapterNotification>();
        foreach (var follower in chapter.Manga.Followers)
        {
            notifications.Add(new ChapterNotification
            {
                Chapter = chapter,
                User = follower,
            });
        }

        _context.Notifications.AddRange(notifications);
        await _context.SaveChangesAsync();

        var notificationDto = _mapper.Map<ChapterNotificationDTO>(notifications[0]);
        await SendToManyUsersAsync(chapter.Manga.Followers.Select(f => f.Id).ToArray(),
            notificationDto);
    }

    public async Task HandleFollowerNotificationAsync(ApplicationUser user)
    {
        var notifications = new List<FollowerNotification>();
        foreach (var follower in user.Followers.Select(f => f.User))
        {
            notifications.Add(new FollowerNotification
            {
                User = user,
                FollowedPerson = follower,
            });
        }

        _context.Notifications.AddRange(notifications);
        await _context.SaveChangesAsync();

        var notificationDto = _mapper.Map<FollowerNotificationDTO>(notifications[0]);
        await SendToManyUsersAsync(user.Followers.Select(f => f.UserId).ToArray(),
            notificationDto);
    }

    public async Task HandleGroupNotificationAsync(Group group)
    {
        var notifications = new List<GroupNotification>();
        foreach (var member in group.Members.Select(m => m.User))
        {
            notifications.Add(new GroupNotification
            {
                User = member,
                Group = group,
            });
        }

        _context.Notifications.AddRange(notifications);
        await _context.SaveChangesAsync();

        var notificationDto = _mapper.Map<GroupNotificationDTO>(notifications[0]);
        await SendToManyUsersAsync(group.Members.Select(f => f.UserId).ToArray(),
            notificationDto);
    }
}
