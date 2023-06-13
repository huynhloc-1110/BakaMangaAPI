using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace BakaMangaAPITest.Controllers;

public class ManageMangaControllerTests : IClassFixture<MangaFixture>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ManageMangaController _controller;

    public ManageMangaControllerTests(MangaFixture fixture)
    {
        _context = fixture.CreateContext();
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AppMapper()));
        _mapper = mapperConfig.CreateMapper();
        var mediaManager = new MockMediaManager();
        _controller = new ManageMangaController(_context, _mapper, mediaManager);
    }

    [Fact]
    public async Task PostManga_ReturnsCreatedAtAction()
    {
        var mangaDTO = new MangaDetailDTO
        {
            Id = Guid.NewGuid().ToString(),
            OriginalTitle = "Your Original Title",
            // Set other properties as needed
        };

        _context.Database.BeginTransaction();
        var result = await _controller.PostManga(mangaDTO, GetMockFile());

        // Check the return type
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var mangaDetailDTO = Assert.IsType<MangaDetailDTO>(createdAtActionResult.Value);

        // Check the return value
        Assert.Equal(mangaDTO.Id, mangaDetailDTO.Id);
        Assert.Equal(mangaDTO.OriginalTitle, mangaDetailDTO.OriginalTitle);

        // Check the database change
        _context.ChangeTracker.Clear();
        var manga = await _context.Mangas.SingleOrDefaultAsync(m => m.Id == mangaDetailDTO.Id);
        Assert.Equal(manga!.OriginalTitle, mangaDetailDTO.OriginalTitle);
        Assert.NotNull(manga!.CoverPath);
        Assert.Equal(manga!.CoverPath, mangaDetailDTO.CoverPath);
    }

    private static IFormFile GetMockFile()
    {
        var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
        return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
    }

    //[Fact]
    //public async Task DeleteManga_ReturnsNoContent()
    //{
    //    var result = await _controller.DeleteManga(_testId);
    
    //    //Find the DateTime of DeletedAt
    //    var searchManga = await _context.Mangas.FindAsync(_testId);
    //    DateTime? deletedAt = searchManga?.DeletedAt;

    //    // Assert
    //    var noContent = Assert.IsType<NoContentResult>(result);

    //    //Expect whether the manga is deleted or not
    //    Assert.NotNull(deletedAt);
    //}

    //[Fact]
    //public async Task PutManga_ReturnsNoContent()
    //{
    //    // Act
    //    var manga = await _context.Mangas.FindAsync(_testId);
    //    var mangaDTO = _mapper.Map<MangaDetailDTO>(manga);
    //    mangaDTO.OriginalTitle = "New Original Title";

    //    var result = await _controller.PutManga(_testId, mangaDTO);
    //    var updatedManga = await _context.Mangas.FindAsync(_testId);

    //    // Assert
    //    var noContent = Assert.IsType<NoContentResult>(result);
    //    Assert.Equal(mangaDTO.OriginalTitle, updatedManga?.OriginalTitle);
    //}
}
