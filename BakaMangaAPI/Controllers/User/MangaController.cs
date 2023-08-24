using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace BakaMangaAPI.Controllers;

[Route("mangas")]
[ApiController]
public class MangaController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public MangaController(ApplicationDbContext context, IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    // GET: /mangas
    [HttpGet]
    public async Task<IActionResult> GetMangas
        ([FromQuery] MangaFilterDTO filter)
    {
        var query = _context.Mangas.AsQueryable();

        // exclude deleted filter
        if (filter.ExcludeDeleted)
        {
            query = query.Where(a => a.DeletedAt == null);
        }

        // search filter
        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(m => m.OriginalTitle.ToLower().Contains(filter.Search.ToLower()) ||
                m.AlternativeTitles!.Contains(filter.Search));
        }

        // category filter
        if (filter.IncludedCategoryIds != null)
        {
            var includedCategoryIds = filter.IncludedCategoryIds.Split(",");
            foreach (var categoryId in includedCategoryIds)
            {
                query = query.Where(m => m.Categories.Any(c => c.Id.StartsWith(categoryId)));
            }
        }
        if (filter.ExcludedCategoryIds != null)
        {
            var excludedCategoryIds = filter.ExcludedCategoryIds.Split(",");
            query = query.Where(m => !m.Categories.Any(c =>
                excludedCategoryIds.Contains(c.Id.Substring(0, 5))));
        }

        var mangasCount = await query.CountAsync();

        // sort options
        query = filter.SortOption switch
        {
            SortOption.Default => query.OrderBy(m => m.OriginalTitle),
            SortOption.LatestChapter => query
                .OrderByDescending(m => m.Chapters.Max(c => c.CreatedAt)),
            SortOption.LatestManga => query
                .OrderByDescending(m => m.CreatedAt),
            SortOption.MostViewDaily => query
                .OrderByDescending(m => m.Chapters.Select(c => c.ChapterViews
                    .Count(cv => DateTime.Equals(cv.CreatedAt.Date, DateTime.UtcNow.Date))).Sum()),
            SortOption.MostView => query
                .OrderByDescending(m => m.Chapters.Select(c => c.ChapterViews.Count).Sum()),
            SortOption.MostFollow => query
                .OrderByDescending(m => m.Followers.Count),
            SortOption.BestRating => query
                .OrderByDescending(m => m.Ratings.Count > 0 ? m.Ratings.Average(r => r.Value) : 3.5),
            _ => throw new ArgumentException("Invalid sort option")
        };

        // page
        var mangas = await query
            .Include(m => m.Categories)
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

    // GET: /mangas/trending
    [HttpGet("trending")]
    public async Task<IActionResult> GetTrendingMangas()
    {
        var topWeeklyViews = await _context.Mangas.MaxAsync(m => m.Chapters
            .Select(c => c.ChapterViews
            .Count(cv => DateTime.Equals(cv.CreatedAt.Date, DateTime.UtcNow.Date)))
            .Sum());
        var trendingMangas = await _context.Mangas
            .Where(m => m.DeletedAt == null)
            .Select(m => new
            {
                Manga = m,
                WeeklyViews = m.Chapters.Select(c => c.ChapterViews
                    .Count(cv => (DateTime.UtcNow - cv.CreatedAt).Days <= 7)).Sum(),
                MaxViews = _context.Mangas.Max(m => m.Chapters.Select(c => c.ChapterViews
                    .Count(cv => (DateTime.UtcNow - cv.CreatedAt).Days <= 7)).Sum()),
                AverageRating = m.Ratings.Count > 0 ? m.Ratings.Average(r => r.Value) : 3.5,
                ExistingWeeks = (DateTime.UtcNow - m.CreatedAt).Days / 7
            })
            .Select(m => new
            {
                m.Manga,
                ViewScore = (float)m.WeeklyViews * 10 / m.MaxViews,
                RatingScore = 2 * m.AverageRating,
                NewScore = 10 - (m.ExistingWeeks < 10 ? m.ExistingWeeks : 10)
            })
            .OrderByDescending(m => 4 * m.ViewScore + 2 * m.RatingScore + 4 * m.NewScore)
            .Select(m => m.Manga)
            .Include(m => m.Categories)
            .Take(10)
            .ToListAsync();
        return Ok(_mapper.Map<List<MangaBasicDTO>>(trendingMangas));
    }

    //GET: /mangas/new-to-you
    [HttpGet("new-to-you")]
    [Authorize]
    public async Task<IActionResult> GetRecommendedMangas()
    {
        var recommendedMangas = await _context.Mangas
            .Where(m => m.DeletedAt == null)
            .OrderBy(m => Guid.NewGuid())
            .Take(12)
            .ToListAsync();
        return Ok(_mapper.Map<List<MangaBasicDTO>>(recommendedMangas));
    }

    // GET: /mangas/5
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

        var result = _mapper.Map<MangaDetailDTO>(manga);

        // get total views of chapters in manga
        result.ViewCount = await _context.Chapters
            .Where(c => c.Manga == manga)
            .SumAsync(c => c.ChapterViews.Count);

        return Ok(result);
    }
}
