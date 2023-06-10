using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class View : BaseModelWithCreatedAt
{
    [Range(0, int.MaxValue)]
    public int Count { get; set; }

    public ApplicationUser User { get; set; } = default!;
}

public class ChapterView : View
{
    public Chapter Chapter { get; set; } = default!;
}

public class CommentView : View
{
    public Comment Comment { get; set; } = default!;
}

public class PostView : View
{
    public Post Post { get; set; } = default!;
}
