namespace BakaMangaAPITest.Fixtures;

public class MangaFixture : BaseFixture
{
    protected override void SeedData(ApplicationDbContext context)
    {
        context.Mangas.AddRange(
            new Manga { Id = Guid.NewGuid().ToString(), OriginalTitle = "One Piece" },
            new Manga { Id = Guid.NewGuid().ToString(), OriginalTitle = "Biography of Hwiyeong Sword" },
            new Manga { Id = Guid.NewGuid().ToString(), OriginalTitle = "Gachiakuta" },
            new Manga { Id = Guid.NewGuid().ToString(), OriginalTitle = "Naruto" },
            new Manga { Id = Guid.NewGuid().ToString(), OriginalTitle = "Bleach" },
            new Manga { Id = Guid.NewGuid().ToString(), OriginalTitle = "Wonderful Days" },
            new Manga { Id = Guid.NewGuid().ToString(), OriginalTitle = "Happiness Rides a Broomstick" },
            new Manga { Id = Guid.NewGuid().ToString(), OriginalTitle = "Please Don't Come to the Villainess' Stationery Store!" },
            new Manga { Id = Guid.NewGuid().ToString(), OriginalTitle = "Giant Ojou-sama" },
            new Manga { Id = Guid.NewGuid().ToString(), OriginalTitle = "Ouji to Himegoto" }
        );
        context.SaveChanges();
    }
}
