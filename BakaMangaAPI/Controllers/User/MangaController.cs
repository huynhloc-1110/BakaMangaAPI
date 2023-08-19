﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using BakaMangaAPI.Models;

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
            SortOption.Default => query.OrderBy(m => m.OriginalTitle),
            SortOption.Trending => throw new NotImplementedException(),
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
            SortOption.NewToYou => query
                .OrderBy(r => Guid.NewGuid()),
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
