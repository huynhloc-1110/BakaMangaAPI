using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakaMangaAPI.Models;

public class Comment : BaseModelWithCreatedAt
{
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = default!;

    public List<CommentReact> Reacts { get; set; } = new();

    public List<CommentReport> Reports { get; set; } = new();

    public string? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }

    public List<Comment> ChildComments { get; set; } = new();
}

public class MangaComment : Comment
{
    public Manga Manga { get; set; } = default!;
}

public class ChapterComment : Comment
{
    public Chapter Chapter { get; set; } = default!;
}

public class PostComment : Comment
{
    public Post Post { get; set; } = default!;
}
