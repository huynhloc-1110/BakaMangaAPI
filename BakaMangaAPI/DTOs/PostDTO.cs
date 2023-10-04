using System.ComponentModel.DataAnnotations;

using BakaMangaAPI.Models;

namespace BakaMangaAPI.DTOs;

public class PostBasicDTO
{
    public string Id { get; set; } = default!;
    public string Content { get; set; } = default!;
    public UserSimpleDTO User { get; set; } = default!;
    public List<string> ImageUrls { get; set; } = new();
    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
    public ReactFlag UserReactFlag { get; set; }
    public int CommentCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PostEditDTO
{
    [MaxLength(2000)]
    public string Content { get; set; } = default!;

    public List<IFormFile> Images { get; set; } = new();
}
