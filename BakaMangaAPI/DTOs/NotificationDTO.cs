namespace BakaMangaAPI.DTOs;

public class NotificationDTO
{
    public string Type { get; set; } = "OtherRequest";
}

public class TestNotificationDTO : NotificationDTO
{
    public string Test { get; set; } = "This is a test property";
}
