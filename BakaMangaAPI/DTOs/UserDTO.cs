using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BakaMangaAPI.DTOs;

public class UserBasicDTO : UserSimpleDTO
{
    public string? BannerPath { get; set; }

    public string Email { get; set; } = default!;

    public List<string> Roles { get; set; } = new();

    public string Biography { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
}

public class UserSimpleDTO
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? AvatarPath { get; set; }
}

public class UserStatsDTO
{
    public int FollowerNumber { get; set; }
    public int FollowingNumber { get; set; }
    public int FollowedMangaNumber { get; set; }
    public int UploadedChapterNumber { get; set; }
    public int ViewGainedNumber { get; set; }
}

public class UserFollowDTO
{
    public UserSimpleDTO User { get; set; } = default!;
    public DateTime FollowedAt { get; set; }
}
