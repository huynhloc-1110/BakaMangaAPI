﻿using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace BakaMangaAPI.Models;

public class ApplicationUser : IdentityUser
{
    [DataType(DataType.ImageUrl)]
    public string? AvatarPath { get; set; }

    [DataType(DataType.ImageUrl)]
    public string? BannerPath { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(1000)]
    public string Biography { get; set; } = default!;

    [DataType(DataType.DateTime)]
    public DateTime? DeletedAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime? BannedUntil { get; set; }

    public List<ApplicationUserRole> UserRoles { get; set; } = new();
    public List<View> Views { get; set; } = new();
    public List<Manga> FollowedMangas { get; set; } = new();
    public List<Chapter> UploadedChapters { get; set; } = new();
    public List<Rating> Ratings { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<React> Reacts { get; set; } = new();
    public List<GroupMember> GroupMembers { get; set; } = new();
    public List<ApplicationUserFollow> Followers { get; set; } = new();
    public List<ApplicationUserFollow> Followings { get; set; } = new();
    public List<MangaList> MangaLists { get; set; } = new();
    public List<MangaListFollower> MangaListFollowers { get; set; } = new();
    public List<Post> Posts { get; set; } = new();
    public List<Request> Requests { get; set; } = new();
    public List<Report> Reports { get; set; } = new();
    public List<Notification> Notifications { get; set; } = new();
}
