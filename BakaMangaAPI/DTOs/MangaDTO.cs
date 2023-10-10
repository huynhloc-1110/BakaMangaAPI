using System.ComponentModel.DataAnnotations;

using BakaMangaAPI.Models;

namespace BakaMangaAPI.DTOs;

public class MangaSimpleDTO
{
    public string Id { get; set; } = default!;
    public string? CoverPath { get; set; }
    public string OriginalTitle { get; set; } = default!;
}

public class MangaBasicDTO : MangaSimpleDTO
{
    public Language OriginalLanguage { get; set; }
    public string? Description { get; set; }
    public List<CategoryDTO> Categories { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}

public class MangaDetailDTO : MangaBasicDTO
{
    public string? AlternativeTitles { get; set; }
    public int PublishYear { get; set; }
    public List<AuthorDTO> Authors { get; set; } = new();
}

public class MangaStatsDTO
{
    public int ViewCount { get; set; }
    public int FollowCount { get; set; }
    public int RatingSum { get; set; }
    public int RatingCount { get; set; }
}

public class MangaEditDTO
{
    public IFormFile? CoverImage { get; set; }

    [MaxLength(250)]
    public string OriginalTitle { get; set; } = string.Empty;

    [RegularExpression(@"^[^;]+(?:; [^;]+)*$")]
    public string? AlternativeTitles { get; set; }

    public Language OriginalLanguage { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(1000, 2100)]
    public int PublishYear { get; set; }

    public string CategoryIds { get; set; } = string.Empty;
    public string AuthorIds { get; set; } = string.Empty;
}

public class MangaBlockDTO : MangaSimpleDTO
{
    public List<ChapterBasicDTO> Chapters { get; set; } = new();

    public DateTime UpdatedAt { get; set; }
}

