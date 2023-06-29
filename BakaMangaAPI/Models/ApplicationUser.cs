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

    public List<ApplicationUser> Followers { get; set; } = new();

    public List<ApplicationUser> Followees { get; set; } = new();

    public List<Request> Requests { get; set; } = new();

    public List<React> Reacts { get; set; } = new();

    public List<Report> Reports { get; set; } = new();

    public List<View> Views { get; set; } = new();
}
