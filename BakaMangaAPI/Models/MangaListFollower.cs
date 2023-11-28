namespace BakaMangaAPI.Models;

public class MangaListFollower
{
    public string MangaListID { get; set; } = default!;
    public MangaList MangaList { get; set; } = default!;

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
}
