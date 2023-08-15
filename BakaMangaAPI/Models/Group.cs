using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

[Index(nameof(Name), IsUnique = true)]
public class Group : BaseModelWithCreatedAt
{
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(1000)]
    public string Biography { get; set; } = default!;

    public List<GroupMember> Members { get; set; } = new();

    public List<Chapter> Chapters { get; set; } = new();
}
