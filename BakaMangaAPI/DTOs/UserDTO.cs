using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BakaMangaAPI.DTOs;

public class UserBasicDTO
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? AvatarPath { get; set; }

    public string? BannerPath { get; set; }

    public string Email { get; set; } = default!;

    public List<string> Roles { get; set; } = new();
}

public class UserSimpleDTO
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;
}
