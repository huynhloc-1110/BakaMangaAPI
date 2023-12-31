namespace BakaMangaAPI.DTOs;

public class UserBasicDTO : UserSimpleDTO
{
    public string? BannerPath { get; set; }

    public string Email { get; set; } = default!;

    public List<string> Roles { get; set; } = new();

    public string Biography { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? BannedUntil { get; set; }
}

public class UserSimpleDTO
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? AvatarPath { get; set; }
    public DateTime? DeletedAt { get; set; }
}

public class UserStatsDTO
{
    public int FollowerNumber { get; set; }
    public int FollowingNumber { get; set; }
    public int FollowedMangaNumber { get; set; }
    public int UploadedChapterNumber { get; set; }
    public int ViewGainedNumber { get; set; }
}

public class UserFollowerDTO
{
    public UserSimpleDTO User { get; set; } = default!;
    public DateTime FollowedAt { get; set; }
}

public class UserFollowingDTO
{
    public UserSimpleDTO FollowedUser { get; set; } = default!;
    public DateTime FollowedAt { get; set; }
}

public class ChangeUsernameDTO
{
    public string Name { get; set; } = default!;
}

public class ChangeUserBioDTO
{
    public string Biography { get; set; } = default!;
}
