﻿using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Comment : BaseModel
{
    [MaxLength(2000)]
    public string Content { get; set; } = default!;

    public ApplicationUser User { get; set; } = default!;
    public string? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }

    public List<CommentReact> Reacts { get; set; } = new();
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
