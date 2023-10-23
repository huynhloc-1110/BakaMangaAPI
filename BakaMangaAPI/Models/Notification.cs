namespace BakaMangaAPI.Models;

public class Notification : BaseModel
{
    public ApplicationUser User { get; set; } = default!;
    public bool IsViewed { get; set; }
}

public class RequestNotification : Notification
{
    public Request Request { get; set; } = default!;
}

public class ChapterNotification : Notification
{
    public Chapter Chapter { get; set; } = default!;
}

public class GroupNotification : Notification
{
    public Group Group { get; set; } = default!;
}

public class FollowerNotification : Notification
{
    public ApplicationUser FollowedPerson { get; set; } = default!;
}
