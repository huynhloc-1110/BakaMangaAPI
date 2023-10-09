using AutoMapper;

using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Follow;

[ApiController]
public class FollowMangaController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public FollowMangaController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet("followed-mangas")]
    [Authorize]
    public async Task<IActionResult> GetMyFollowedMangas([FromQuery] FilterDTO filter)
    {
        var user = await _userManager.GetUserAsync(User);
        var chapterGroupingCount = await _context.Mangas
            .Where(m => m.Followers.Contains(user))
            .CountAsync();

        var chapterGroupings = await _context.Chapters
            .Where(c => c.Manga.Followers.Contains(user))
            .Include(c => c.Uploader)
            .Include(c => c.UploadingGroup)
            .Include(c => c.ChapterViews)
            .GroupBy(c => c.Manga.Id)
            .Select(g => new
            {
                Manga = _context.Mangas.Single(m => m.Id == g.Key),
                Chapters = g.OrderByDescending(c => c.CreatedAt).Take(3).ToList(),
                UpdatedAt = g.Max(c => c.CreatedAt)
            })
            .OrderByDescending(g => g.UpdatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        var chapterGroupingList = chapterGroupings
            .Select(g => new ChapterGroupingDTO()
            {
                Manga = _mapper.Map<MangaSimpleDTO>(g.Manga),
                Chapters = _mapper.Map<List<ChapterBasicDTO>>(g.Chapters)
            })
            .ToList();

        var paginatedList = new PaginatedListDTO<ChapterGroupingDTO>
            (chapterGroupingList, chapterGroupingCount, filter.Page, filter.PageSize);
        return Ok(paginatedList);
    }
}