using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Author : BaseModel
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Biography { get; set; } = string.Empty;

    public List<Manga> Mangas { get; set; } = new();
}
