using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BakaMangaAPI.Controllers;

[Route("user/[controller]")]
[ApiController]
public class MangaController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public MangaController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Manga
    [HttpGet]
    public async Task<IActionResult> GetMangas
        ([FromQuery] MangaFilterDTO filter)
    {
        var query = _context.Mangas.Where(m => m.DeletedAt == null);

        // search
        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(m => m.OriginalTitle.ToLower().Contains(filter.Search.ToLower()) ||
                m.AlternativeTitles!.Contains(filter.Search));
        }
        var mangasCount = await query.CountAsync();

        // sort options
        query = filter.SortOption switch
        {
            SortOption.LatestChapter => query
                .Include(m => m.Chapters)
                .OrderByDescending(m => m.Chapters.Max(c => c.CreatedAt)),
            SortOption.LatestManga => query
                .OrderByDescending(m => m.CreatedAt),
            _ => throw new NotImplementedException()
        };

        // page
        var mangas = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsNoTracking()
            .ToListAsync();
        if (mangas.Count == 0)
        {
            return NotFound();
        }

        var mangaList = _mapper.Map<List<MangaBasicDTO>>(mangas);
        var paginatedMangaList = new PaginatedListDTO<MangaBasicDTO>
            (mangaList, mangasCount, filter.Page, filter.PageSize);
        return Ok(paginatedMangaList);
    }

    // GET: api/Manga/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetManga(string id)
    {
        var manga = await _context.Mangas
            .Include(m => m.Categories)
            .Where(m => m.DeletedAt == null)
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);

        if (manga == null || manga.DeletedAt != null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<MangaDetailDTO>(manga));
    }
}
