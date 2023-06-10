using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Image : BaseModel
{
    [DataType(DataType.ImageUrl)]
    public string Path { get; set; } = string.Empty;

    public Chapter Chapter { get; set; } = default!;
}
