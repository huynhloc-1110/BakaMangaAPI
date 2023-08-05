using BakaMangaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.DTOs;

public class CommentDTO
{
    public string Id { get; set; } = default!;

    public string Content { get; set; } = default!;

    public UserBasicDTO User { get; set; } = default!;

    public int LikeCount { get; set; }

    public int DislikeCount { get; set; }

    public int UserReactFlag { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ChildCommentCount { get; set; }

    public string? ParentCommentId { get; set; }
}

public class CommentEditDTO
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [MaxLength(2000)]
    public string Content { get; set; } = default!;
}