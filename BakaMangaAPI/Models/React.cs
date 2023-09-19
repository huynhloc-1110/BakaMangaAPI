namespace BakaMangaAPI.Models;

public class React : BaseModel
{
    public ReactFlag ReactFlag { get; set; }

    public ApplicationUser User { get; set; } = default!;
}

public enum ReactFlag
{
    Dislike = -1, Like = 1
}

public class CommentReact : React
{
    public Comment Comment { get; set; } = default!;
}

public class PostReact : React
{
    public Post Post { get; set; } = default!;
}
