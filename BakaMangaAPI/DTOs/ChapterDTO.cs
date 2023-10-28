using BakaMangaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.DTOs;

public class ChapterSimpleDTO
{
    public string Id { get; set; } = default!;

    public float Number { get; set; }

    public string Name { get; set; } = default!;

    public MangaBasicDTO Manga { get; set; } = default!;

    public GroupSimpleDTO UploadingGroup { get; set; } = default!;
}

public class ChapterBasicDTO : ChapterSimpleDTO
{
    public DateTime CreatedAt { get; set; }

    public Language Language { get; set; }

    public UserSimpleDTO Uploader { get; set; } = default!;

    public int ViewCount { get; set; }

    public bool IsViewedByUser { get; set; }
}

public class ChapterDetailDTO : ChapterBasicDTO
{
    public List<string> PageUrls { get; set; } = new();

    public DateTime? DeletedAt { get; set; }
}

public class ChapterEditDTO
{
    [Range(0, float.MaxValue)]
    public float Number { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public Language Language { get; set; }

    public List<IFormFile> Pages { get; set; } = new();

    public string UploadingGroupId { get; set; } = default!;
}
