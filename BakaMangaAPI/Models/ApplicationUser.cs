using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class ApplicationUser : IdentityUser
{
    [DataType(DataType.DateTime)]
    public DateTime? DeletedAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public List<ApplicationUserRole> UserRoles { get; set; } = new();

    #region Have yet used these
    public List<ApplicationUser> Followers { get; set; } = new();

    public List<ApplicationUser> Followees { get; set; } = new();

    public List<Request> Requests { get; set; } = new();

    public List<Report> Reports { get; set; } = new();
    #endregion

    public List<ChapterView> ChapterViews { get; set; } = new();

    public List<Manga> FollowedMangas { get; set; } = new();

    public List<Chapter> UploadedChapters { get; set; } = new();

    public List<Rating> Ratings { get; set; } = new();

    public List<MangaComment> MangaComments { get; set; } = new();

    public List<CommentReact> CommentReacts { get; set; } = new();
}
