using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class MangaListItem
{
    public string MangaListId { get; set; } = default!;
    public MangaList MangaList { get; set; } = default!;

    public string MangaId { get; set; } = default!;
    public Manga Manga { get; set; } = default!;

    [Range(0, int.MaxValue)]
    public int Index { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
