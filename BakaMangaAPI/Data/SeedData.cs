using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace BakaMangaAPI.Data;

public partial class SeedData : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public SeedData(IServiceProvider serviceProvider)
    {
        _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        _configuration = serviceProvider.GetRequiredService<IConfiguration>();
        _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        _roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    }

    public void Initialize()
    {
        if (!_context.ApplicationUsers.Any())
        {
            SeedUsersAsync().Wait();
        }
        if (!_context.Mangas.Any())
        {
            var categories = SeedCategories();
            var authors = SeedAuthors();
            var groups = SeedGroups();
            SeedMangas(categories, authors, groups);
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
