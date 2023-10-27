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
                AvatarPath = "https://res.cloudinary.com/diprbtdq4/image/upload/v1698376050/avatars/huy.jpg",
                Biography = "Huy Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."

            },
            new() {
                Name = "Loc Le",
                Email = "LocLe345@example.com",
                UserName = "LocLe345@example.com",
                AvatarPath = "https://res.cloudinary.com/diprbtdq4/image/upload/v1698376053/avatars/loc.jpg",
                Biography = "Loc Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            },
            new() {
                Name = "Tri Tat",
                Email = "TriTat567@example.com",
                UserName = "TriTat567@example.com",
                AvatarPath = "https://res.cloudinary.com/diprbtdq4/image/upload/v1698376051/avatars/tri.jpg",
                Biography = "Tri Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            },
            new() {
                Name = "Khoa Le",
                Email = "KhoaLe789@example.com",
                UserName = "KhoaLe789@example.com",
                AvatarPath = "https://res.cloudinary.com/diprbtdq4/image/upload/v1698376050/avatars/khoa.jpg",
                Biography = "Khoa Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            },
            new() {
                Name = "Admin",
                Email = "Admin1@example.com",
                UserName = "Admin1@example.com",
                AvatarPath = "https://res.cloudinary.com/diprbtdq4/image/upload/v1698376049/avatars/admin.jpg",
                Biography = "Admin Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            },
             new() {
                Name = "Uploader",
                Email = "Uploader1@example.com",
                UserName = "Uploader1@example.com",
                AvatarPath = "https://res.cloudinary.com/diprbtdq4/image/upload/v1698376050/avatars/uploader.jpg",
                Biography = "Uploader Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            },
        };

        string[][] roleAssignments =
        {
            new[] { "User" },
            new[] { "Uploader" },
            new[] { "Manager" },
            new[] { "Admin" },
            new[] { "User", "Uploader", "Manager", "Admin" },
            new[] { "Uploader" },
        };

        for (int i = 0; i < users.Count; i++)
        {
            await _userManager.CreateAsync(users[i], users[i].Email);
            foreach (string role in roleAssignments[i])
            {
                await _userManager.AddToRoleAsync(users[i], role);
            }
        }
    }
}
