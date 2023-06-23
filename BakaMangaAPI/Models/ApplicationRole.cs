using Microsoft.AspNetCore.Identity;

namespace BakaMangaAPI.Models;

public class ApplicationRole : IdentityRole
{
    public List<ApplicationUserRole> UserRoles { get; set; } = new();

    public ApplicationRole(string role) : base(role) { }

    public ApplicationRole() : base() { }
}
