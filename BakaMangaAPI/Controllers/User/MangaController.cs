using AutoMapper;
using AutoMapper.QueryableExtensions;

using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers;

[Route("mangas")]
[ApiController]
public partial class MangaController : ControllerBase
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
        if (filter.IncludedCategoryIds.Count > 0)
        {
            foreach (var categoryId in filter.IncludedCategoryIds)
            {
                query = query.Where(m => m.Categories.Any(c => c.Id.StartsWith(categoryId)));
            }
        }
        if (filter.ExcludedCategoryIds.Count > 0)
        {
            query = query.Where(m => !m.Categories.Any(c =>
                filter.ExcludedCategoryIds.Contains(c.Id.Substring(0, 5))));
        }

        // language filter
        if (filter.SelectedLanguages.Count > 0)
        {
            query = query.Where(m => filter.SelectedLanguages.Contains(m.OriginalLanguage));
        }

        // author filter
        if (!string.IsNullOrEmpty(filter.SelectedAuthorId))
        {
            query = query.Where(m => m.Authors.Any(a => a.Id == filter.SelectedAuthorId));
        }

        var mangasCount = await query.CountAsync();

        // sort options
        query = filter.SortOption switch
        {
            SortOption.Default => query.OrderBy(m => m.OriginalTitle),
            SortOption.LatestChapter => query
                .OrderByDescending(m => m.Chapters.Any() ?
                    m.Chapters.Max(c => c.CreatedAt) : DateTime.MinValue),
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
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<MangaBasicDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        var paginatedMangaList = new PaginatedListDTO<MangaBasicDTO>
            (mangas, mangasCount, filter.Page, filter.PageSize);
        return Ok(paginatedMangaList);
    }

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
                ViewScore = (m.MaxViews > 0) ? (float)m.WeeklyViews * 10 / m.MaxViews : 0,
                RatingScore = 2 * m.AverageRating,
                NewScore = 10 - (m.ExistingWeeks < 10 ? m.ExistingWeeks : 10)
            })
            .OrderByDescending(m => 4 * m.ViewScore + 2 * m.RatingScore + 4 * m.NewScore)
            .Select(m => m.Manga)
            .Take(10)
            .ProjectTo<MangaBasicDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        return Ok(trendingMangas);
    }

    [HttpGet("new-to-you")]
    [Authorize]
    public async Task<IActionResult> GetRecommendedMangas()
    {
        var recommendedMangas = await _context.Mangas
            .Where(m => m.DeletedAt == null)
            .OrderBy(m => EF.Functions.Random())
            .Take(12)
            .ProjectTo<MangaBasicDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        return Ok(recommendedMangas);
    }

    [HttpGet("{mangaId}")]
    public async Task<IActionResult> GetManga(string mangaId)
    {
        var manga = await _context.Mangas
            .Where(m => m.DeletedAt == null)
            .Where(m => m.Id == mangaId)
            .ProjectTo<MangaDetailDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (manga == null)
        {
            return NotFound();
        }

        return Ok(manga);
    }

    [HttpGet("{mangaId}/stats")]
    public async Task<IActionResult> GetMangaStats(string mangaId)
    {
        var mangaStats = await _context.Mangas
            .Where(m => m.Id == mangaId)
            .ProjectTo<MangaStatsDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (mangaStats == null)
        {
            return NotFound();
        }
        return Ok(mangaStats);
    }
}
