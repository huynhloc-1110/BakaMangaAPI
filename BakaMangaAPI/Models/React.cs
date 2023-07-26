namespace BakaMangaAPI.Models;

public class React : BaseModelWithCreatedAt
{
    public ReactFlag ReactFlag { get; set; }

    public ApplicationUser User { get; set; } = default!;

    public string UserId { get; set; } = default!;
}

public enum ReactFlag
{
    Dislike = -1, NoReact, Like
}

public class CommentReact : React
{
    public Comment Comment { get; set; } = default!;
}

public class PostReact : React
{
    public Post Post { get; set; } = default!;
}
