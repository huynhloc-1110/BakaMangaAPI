using System.ComponentModel.DataAnnotations;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.DTOs;

public class MangaBasicDTO
{
    public string Id { get; set; } = default!;

    public string? CoverPath { get; set; }

    public string OriginalTitle { get; set; } = default!;

    public Language OriginalLanguage { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}

public class MangaEditDTO
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

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

public class MangaDetailDTO
{
    public string Id { get; set; } = default!;

    public string? CoverPath { get; set; }

    public string OriginalTitle { get; set; } = default!;

    public string? AlternativeTitles { get; set; }

    public Language OriginalLanguage { get; set; }

    public string? Description { get; set; }

    public int PublishYear { get; set; }

    public DateTime CreatedAt { get; set; } = default!;

    public List<CategoryDTO> Categories { get; set; } = new();

    public List<AuthorDTO> Authors { get; set; } = new();

    public int ViewCount { get; set; }

    public int FollowCount { get; set; }

    public float AverageRating { get; set; }
}
