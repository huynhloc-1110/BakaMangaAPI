using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Author : BaseModel
{
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(1000)]
    public string Biography { get; set; } = default!;

    public List<Manga> Mangas { get; set; } = new();
}
