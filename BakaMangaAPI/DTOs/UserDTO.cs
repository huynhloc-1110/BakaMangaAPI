using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BakaMangaAPI.DTOs;

public class UserBasicDTO
{
    public string Id { get; set; } = default!;

    public string Email { get; set; } = default!;

	public List<string> Roles { get; set; } = new();
}
