using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPITest.Fixtures;

public abstract class BaseFixture
{
    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public BaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using var context = CreateContext();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                SeedData(context);
                _databaseInitialized = true;
            }
        }
    }

    public ApplicationDbContext CreateContext()
        => new(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(TestConfiguration.DbConnectionString)
            .Options);

    protected abstract void SeedData(ApplicationDbContext context);
}
