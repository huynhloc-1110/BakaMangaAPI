using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Image : BaseModel
{
    [Range(1, int.MaxValue)]
    public int Number { get; set; }

    [DataType(DataType.ImageUrl)]
    public string Path { get; set; } = string.Empty;
}

public class ChapterImage : Image
{
    public Chapter Chapter { get; set; } = default!;
}

public class PostImage : Image
{
    public Post Post { get; set; } = default!;
}
