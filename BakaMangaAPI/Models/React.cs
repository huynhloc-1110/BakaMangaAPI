using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Models;

public class React : BaseModel
{
    public ReactFlag ReactFlag { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
}

public enum ReactFlag
{
    Dislike = -1, Like = 1, None = 0
}

[Index(nameof(CommentId), nameof(UserId), IsUnique = true)]
public class CommentReact : React
{
    public string CommentId { get; set; } = default!;
    public Comment Comment { get; set; } = default!;
}

[Index(nameof(PostId), nameof(UserId), IsUnique = true)]
public class PostReact : React
{
    public string PostId { get; set; } = default!;
    public Post Post { get; set; } = default!;
}
