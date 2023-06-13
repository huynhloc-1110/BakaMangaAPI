using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BakaMangaAPI.Data;

public partial class SeedData : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    public SeedData(IServiceProvider serviceProvider)
    {
        _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        _configuration = serviceProvider.GetRequiredService<IConfiguration>();
    }

    public void Initialize()
    {
        if (_context.Mangas.Any())
        {
            return;
        }

        var categories = SeedCategories();
        var authors = SeedAuthors();
        SeedMangas(categories, authors);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
