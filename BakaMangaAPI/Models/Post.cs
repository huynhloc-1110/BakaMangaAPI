using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Post : BaseModel
{
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;

    [DataType(DataType.ImageUrl)]
    public string? PicturePath { get; set; }

    public ApplicationUser User { get; set; } = default!;

    public List<PostComment> Comments { get; set; } = new();

    public List<PostReact> Reacts { get; set; } = new();

    public List<PostReport> Reports { get; set; } = new();
}
