using Microsoft.AspNetCore.Mvc;
using AutoMapper;

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
    [InlineData(1, 4)]
    [InlineData(2, 4)]
    [InlineData(3, 4)]
    [InlineData(4, 4)]
    [InlineData(1, 12)]
    public async Task GetMangas_ReturnsListOfMangaBasicDTOs(int page, int itemPerPage)
    {
        var result = await _controller.GetMangas("latest-manga", page, itemPerPage);

        var count = _context.Mangas.Count();
        var itemLeft = count - (page - 1) * itemPerPage;
        var expected = itemLeft < 0 ? 0 : (itemLeft < itemPerPage ? itemLeft : itemPerPage);


        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<MangaBasicDTO>>>(result);
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
        var actual = await _controller.GetManga(testId);

        Assert.Equal(expected!.OriginalTitle, actual.Value!.OriginalTitle);
    }

    [Fact]
    public void GetMangaCount_ReturnsNumberOfMangas()
    {
        var expect = _controller.GetMangaCount().Result.Value;
        var actual = _context.Mangas.Count();

        Assert.Equal(actual, expect);
    }
}
