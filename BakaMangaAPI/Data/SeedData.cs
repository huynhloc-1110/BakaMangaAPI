using BakaMangaAPI.Models;

namespace BakaMangaAPI.Data;

public class SeedData : IDisposable
{
    private readonly ApplicationDbContext _context;
    public SeedData(IServiceProvider serviceProvider)
    {
        _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    public void Initialize()
    {
        if (_context.Mangas.Any())
        {
            return;
        }

        Manga doraemon = new()
        {
            Description = "A cat robot manga",
            OriginalTitle = "Doraemon",
            PublishYear = 1969,
        };

        Manga naruto = new()
        {
            Description = "An edgy ninja story",
            OriginalTitle = "Naruto",
            PublishYear = 1999,
        };

        Chapter chapter = new()
        {
            Id = Guid.NewGuid().ToString(),
            Manga = doraemon,
            Name = "Ch.01 - Meet Doraemon",
        };

        Image image = new()
        {
            Id = Guid.NewGuid().ToString(),
            Path = "doraemon.png",
            Chapter = chapter
        };

        _context.AddRange(doraemon, naruto, chapter, image);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
