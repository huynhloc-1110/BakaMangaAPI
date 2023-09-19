using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Category : BaseModel
{
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(1000)]
    public string Description { get; set; } = default!; 

    public List<Manga> Mangas { get; set; } = new();
}
