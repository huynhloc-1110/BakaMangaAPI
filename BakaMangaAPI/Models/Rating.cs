using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Rating : BaseModel
{
    [Range(1, 5)]
    public int Value { get; set; }

    public ApplicationUser User { get; set; } = default!;

    public Manga Manga { get; set; } = default!;
}
