using BakaMangaAPI.Models;

namespace BakaMangaAPI.Data;

public partial class SeedData
{
    private async Task SeedUsersAsync()
    {
        var roles = new[] { "User", "Uploader", "Manager", "Admin" };
        foreach (var role in roles)
        {
            await _roleManager.CreateAsync(new(role));
        }

        List<ApplicationUser> users = new()
        {
            new() {
                Name = "Huy Nguyen",
                Email = "HuyNguyen123@example.com",
                UserName = "HuyNguyen123@example.com",
            },
            new() {
                Name = "Loc Le",
                Email = "LocLe345@example.com",
                UserName = "LocLe345@example.com",
            },
            new() {
                Name = "Tri Tat",
                Email = "TriTat567@example.com",
                UserName = "TriTat567@example.com",
            },
            new() {
                Name = "Khoa Le",
                Email = "KhoaLe789@example.com",
                UserName = "KhoaLe789@example.com",
            },
        };

        var roleIndex = 0;
        foreach (var user in users)
        {
            await _userManager.CreateAsync(user, user.Email);
            await _userManager.AddToRoleAsync(user, roles[roleIndex]);
            roleIndex++;
        }
    }
}
