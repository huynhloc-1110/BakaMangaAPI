using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class View : BaseModelWithCreatedAt
{
    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
}

[Index(nameof(UserId), nameof(ChapterId), IsUnique = true)]
public class ChapterView : View
{
    public string ChapterId { get; set; } = default!;
    public Chapter Chapter { get; set; } = default!;
}

[Index(nameof(UserId), nameof(PostId), IsUnique = true)]
public class PostView : View
{
    public string PostId { get; set; } = default!;
    public Post Post { get; set; } = default!;
}
