using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Models;

[Index(nameof(UserId), nameof(MangaId), IsUnique = true)]
public class Rating : BaseModel
{
    [Range(1, 5)]
    public int Value { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string MangaId { get; set; } = default!;
    public Manga Manga { get; set; } = default!;
}
