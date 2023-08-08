using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Page : BaseModel
{
    [Range(1, int.MaxValue)]
    public int Number { get; set; }

    [DataType(DataType.ImageUrl)]
    public string Path { get; set; } = string.Empty;

    public Chapter Chapter { get; set; } = default!;
}
