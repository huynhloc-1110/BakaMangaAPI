using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class View : BaseModel
{
    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
}

[Index(nameof(ChapterId), nameof(UserId), IsUnique = true)]
public class ChapterView : View
{
    public string ChapterId { get; set; } = default!;
    public Chapter Chapter { get; set; } = default!;
}

[Index(nameof(PostId), nameof(UserId), IsUnique = true)]
public class PostView : View
{
    public string PostId { get; set; } = default!;
    public Post Post { get; set; } = default!;
}
