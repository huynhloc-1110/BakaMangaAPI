using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.DTOs;

public class GroupBasicDTO
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public int MemberNumber { get; set; }

    public string? AvatarPath { get; set; }
}

public class GroupDetailDTO : GroupBasicDTO
{
    public string? BannerPath { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UploadedChapterNumber { get; set; }

    public int ViewGainedNumber { get; set; } 
}

public class GroupEditDTO
{
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(1000)]
    public string Biography { get; set; } = default!;

    public IFormFile? AvatarImage { get; set; }

    public IFormFile? BannerImage { get; set; }
}
