using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Models;

[Index(nameof(Name), IsUnique = true)]
public class Group : BaseModel
{
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(1000)]
    public string Biography { get; set; } = default!;

    [DataType(DataType.ImageUrl)]
    public string? AvatarPath { get; set; }

    [DataType(DataType.ImageUrl)]
    public string? BannerPath { get; set; }

    public List<GroupMember> Members { get; set; } = new();
    public List<Chapter> Chapters { get; set; } = new();
}
