using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Manga : BaseModelWithCreatedAt
{
    [DataType(DataType.ImageUrl)]
    public string? CoverPath { get; set; }

    [MaxLength(250)]
    public string OriginalTitle { get; set; } = string.Empty;

    public Language OriginalLanguage { get; set; }

    [RegularExpression(@"^[^;]+(?:; [^;]+)*$", ErrorMessage =
        "The titles don't match the required format 'title1; title2; title3'")]
    public string? AlternativeTitles { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(1000, 2100)]
    public int PublishYear { get; set; }

    public List<Category> Categories { get; set; } = new();

    public List<Author> Authors { get; set; } = new();

    public List<Chapter> Chapters { get; set; } = new();

    public List<MangaComment> Comments { get; set; } = new();

    public List<ApplicationUser> Followers { get; set; } = new();

    public List<Rating> Ratings { get; set; } = new();

    public List<MangaListItem> MangaListItems { get; set; } = new();
}
