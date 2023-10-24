using BakaMangaAPI.Models;

namespace BakaMangaAPI.DTOs;

public class NotificationDTO
{
    public string Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public bool IsViewed { get; set; }
}

public class RequestNotificationDTO : NotificationDTO
{
    public string Type { get; set; } = nameof(RequestNotification);
    public string RequestType { get; set; } = default!;
}

public class ChapterNotificationDTO : NotificationDTO
{
    public string Type { get; set; } = nameof(ChapterNotification);
    public ChapterSimpleDTO Chapter { get; set; } = default!;
}

public class GroupNotificationDTO : NotificationDTO
{
    public string Type { get; set; } = nameof(GroupNotification);
    public GroupBasicDTO Group { get; set; } = default!;
}

public class FollowerNotificationDTO : NotificationDTO
{
    public string Type { get; set; } = nameof(FollowerNotification);
    public UserSimpleDTO FollowedPerson { get; set; } = default!;
}
