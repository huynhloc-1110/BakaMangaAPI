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
            .Include(m => m.Authors)
            .Include(m => m.Categories)
            .Include(m => m.Ratings)
            .Include(m => m.Followers)
            .Where(m => m.DeletedAt == null)
            .AsSplitQuery()
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);

        if (manga == null || manga.DeletedAt != null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<MangaDetailDTO>(manga));
    }

    [HttpGet("{id}/chapters")]
    public async Task<IActionResult> GetMangaChapters(string id, [FromQuery]
        FilterDTO filter)
    {
        // filter search
        var query = _context.Chapters.Where(c => c.DeletedAt == null);
        if (!string.IsNullOrEmpty(filter.Search))
        {
            if (int.TryParse(filter.Search, out int searchNum))
            {
                query = query.Where(c => c.Number == searchNum ||
                    c.Name.Contains(filter.Search));
            }
            else
            {
                query = query.Where(c => c.Name.ToLower()
                    .Contains(filter.Search.ToLower()));
            }
        }

        // filter manga
        var manga = await _context.Mangas.FindAsync(id);
        query = query
            .Include(c => c.Manga)
            .Where(c => c.Manga == manga);

        // count total chapter numbers
        var chapterNumberCount = await query
            .Select(c => c.Number)
            .Distinct()
            .CountAsync();

        // get chapters based on filter.Page and PageSize
        var chapterNumberFilter = query
            .Select(c => c.Number)
            .Distinct().OrderBy(cn => cn)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize);
        var chapters = await query
            .Where(c => chapterNumberFilter.Contains(c.Number))
            .Include(c => c.Uploader)
            .Include(c => c.ChapterViews)
            .OrderBy(c => c.Number)
            .ThenBy(c => c.Language)
            .ThenByDescending(c => c.CreatedAt)
            .AsNoTracking()
            .ToListAsync();

        if (chapters.Count == 0)
        {
            return NotFound();
        }

        var chapterList = _mapper.Map<List<ChapterBasicDTO>>(chapters);
        var paginatedChapterList = new PaginatedListDTO<ChapterBasicDTO>
            (chapterList, chapterNumberCount, filter.Page, filter.PageSize);
        return Ok(paginatedChapterList);
    }
}
