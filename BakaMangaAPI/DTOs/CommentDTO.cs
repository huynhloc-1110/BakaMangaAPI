using BakaMangaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.DTOs;

public class CommentDTO
{
    public string Id { get; set; } = default!;

    public string Content { get; set; } = default!;

    public UserSimpleDTO User { get; set; } = default!;

    public int LikeCount { get; set; }

    public int DislikeCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<CommentDTO> ChildComments { get; set; } = new();
}
