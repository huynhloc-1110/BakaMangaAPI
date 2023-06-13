using System.ComponentModel.DataAnnotations;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.DTOs;

public class MangaBasicDTO
{
    public string Id { get; set; } = default!;

    public string? CoverPath { get; set; }

    public string OriginalTitle { get; set; } = string.Empty;

    public Language OriginalLanguage { get; set; }

    public string? Description { get; set; }
}

public class MangaDetailDTO
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? CoverPath { get; set; }

    [MaxLength(250)]
    public string OriginalTitle { get; set; } = string.Empty;

    [RegularExpression(@"^[^;]+(?:; [^;]+)*$")]
    public string AlternativeTitles { get; set; } = string.Empty;

    public Language OriginalLanguage { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(1000, 2100)]
    public int PublishYear { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
