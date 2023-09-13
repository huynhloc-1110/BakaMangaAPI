using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class ApplicationUser : IdentityUser
{
    [DataType(DataType.ImageUrl)]
    public string? BannerPath { get; set; }

    [DataType(DataType.ImageUrl)]
    public string? AvatarPath { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? DeletedAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Biography { get; set; } = string.Empty;

    public List<ApplicationUserRole> UserRoles { get; set; } = new();

    #region Have yet used these
    public List<Request> Requests { get; set; } = new();

    public List<Report> Reports { get; set; } = new();
    #endregion

    public List<View> Views { get; set; } = new();

    public List<Manga> FollowedMangas { get; set; } = new();

    public List<Chapter> UploadedChapters { get; set; } = new();

    public List<Rating> Ratings { get; set; } = new();

    public List<Comment> Comments { get; set; } = new();

    public List<React> Reacts { get; set; } = new();

    public List<GroupMember> GroupMembers { get; set; } = new();

    public List<ApplicationUser> Followers { get; set; } = new();

    public List<ApplicationUser> Followings { get; set; } = new();

    public List<MangaList> MangaLists { get; set; } = new();

    public List<MangaListFollower> MangaListFollowers { get; set; } = new();
}
