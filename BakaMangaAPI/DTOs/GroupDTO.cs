using System.ComponentModel.DataAnnotations;

using BakaMangaAPI.Models;

namespace BakaMangaAPI.DTOs;

public class GroupBasicDTO
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? AvatarPath { get; set; }
    public int MemberNumber { get; set; }
    public bool IsMangaGroup { get; set; }
    public DateTime? UserJoinedAt { get; set; }
}

public class GroupDetailDTO : GroupBasicDTO
{
    public string? BannerPath { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Biography { get; set; } = default!;
    public int UploadedChapterNumber { get; set; }
    public int ViewGainedNumber { get; set; }
}

public class GroupEditDTO
{
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(1000)]
    public string Biography { get; set; } = default!;

    public bool IsMangaGroup { get; set; }
}

public class GroupMemberDTO
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? AvatarPath { get; set; }
    public GroupRole GroupRoles { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
