using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Models;

[Index(nameof(UserId), nameof(FollowedUserId), IsUnique = true)]
public class ApplicationUserFollow
{
    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string FollowedUserId { get; set; } = default!;
    public ApplicationUser FollowedUser { get; set; } = default!;

    public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
}