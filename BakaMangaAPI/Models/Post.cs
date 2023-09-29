using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Post : BaseModel
{
    [MaxLength(2000)]
    public string Content { get; set; } = default!;

    public ApplicationUser User { get; set; } = default!;

    public List<PostImage> Images { get; set; } = new();
    public List<PostView> Views { get; set; } = new();
    public List<PostReact> Reacts { get; set; } = new();
    public List<PostComment> Comments { get; set; } = new();
    public List<PostReport> Reports { get; set; } = new();
}
