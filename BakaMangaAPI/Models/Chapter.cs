using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Chapter : BaseModelWithCreatedAt
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public Language Language { get; set; }

    public List<Image> Images { get; set; } = new();

    public Manga Manga { get; set; } = default!;

    public List<ChapterComment> Comments { get; set; } = new();

    public List<ChapterReport> Reports { get; set; } = new();

    // uploader

    // upload group
}
