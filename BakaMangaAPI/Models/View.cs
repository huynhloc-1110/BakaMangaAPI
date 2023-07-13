using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class View : BaseModelWithCreatedAt
{
    public ApplicationUser User { get; set; } = default!;

    public string UserId { get; set; } = default!;
}

public class ChapterView : View
{
    public Chapter Chapter { get; set; } = default!;
}

public class PostView : View
{
    public Post Post { get; set; } = default!;
}
