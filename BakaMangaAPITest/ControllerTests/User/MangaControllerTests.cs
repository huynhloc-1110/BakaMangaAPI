using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPITest.Controllers;

public class MangaControllerTests : IClassFixture<MangaFixture>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly MangaController _controller;

    public MangaControllerTests(MangaFixture fixture)
    {
        //Reusable context
        _context = fixture.CreateContext();
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AppMapper()));
        _mapper = mapperConfig.CreateMapper();
        _controller = new MangaController(_context, _mapper);
    }

    [Theory]
    [InlineData(4, 1, 4)]
    [InlineData(4, 2, 4)]
    [InlineData(2, 3, 4)]
    [InlineData(0, 4, 4)]
    [InlineData(10, 1, 12)]
    [InlineData(1, null, null, "Naruto")]
    [InlineData(1, null, null, "naruto")]
    [InlineData(0, null, null, "Slam Dunk")]
    public async Task GetMangas_ReturnsListOfMangaBasicDTOs(int expected, int? page = null, int? pageSize = null, string? search = null)
    {
        MangaAdvancedFilterDTO filter = new();
        filter.Page = page ?? filter.Page;
        filter.PageSize = pageSize ?? filter.PageSize;
        filter.Search = search ?? filter.Search;

        var result = await _controller.GetMangas(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var dtoList = Assert.IsAssignableFrom<IEnumerable<MangaBasicDTO>>(okResult.Value);
        Assert.Equal(expected, dtoList.Count());
    }

    [Fact]
    public async Task GetManga_ReturnsMangaDetailDTO()
    {
        // Get random manga using DbContext
        var count = _context.Mangas.Count();
        var skip = Random.Shared.Next(count);
        var expected = _context.Mangas.Skip(skip).Take(1).FirstOrDefault();
        var testId = expected!.Id;

        _context.ChangeTracker.Clear();
        var result = await _controller.GetManga(testId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actual = Assert.IsAssignableFrom<MangaDetailDTO>(okResult.Value);
        Assert.Equal(expected!.OriginalTitle, actual!.OriginalTitle);
    }

    [Fact]
    public async Task GetMangaCount_ReturnsNumberOfMangas()
    {
        var expect =  (await _controller.GetMangaCount()).Value;

        _context.ChangeTracker.Clear();
        var actual = await _context.Mangas.CountAsync(m => m.DeletedAt == null);

        // Assert
        Assert.Equal(actual, expect);
    }
}
