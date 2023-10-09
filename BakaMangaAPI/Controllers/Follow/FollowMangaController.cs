using System.Security.Claims;

using AutoMapper;

using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Follow;

[Route("mangas/{mangaId}/my-follow")]
[ApiController]
[Authorize]
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

    [HttpGet("~/followed-mangas")]
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

    [HttpGet]
    public async Task<bool> GetMyFollowForManga(string mangaId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _context.Mangas
            .AnyAsync(m => m.Id == mangaId &&
                m.Followers.Select(f => f.Id).Any(id => id == userId));
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> PostMyFollowForManga(string mangaId)
    {
        var manga = await _context.Mangas.FindAsync(mangaId);
        if (manga == null)
        {
            return NotFound("Manga not found");
        }

        if (await GetMyFollowForManga(mangaId))
        {
            return BadRequest("User has already followed this manga.");
        }

        var currentUser = await _userManager.GetUserAsync(User);
        manga.Followers.Add(currentUser);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMyFollowForManga), new { mangaId }, true);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveMyFollowForManga(string mangaId)
    {
        var manga = await _context.Mangas
            .Include(m => m.Followers)
            .SingleOrDefaultAsync(m => m.Id == mangaId);
        if (manga == null)
        {
            return BadRequest("Invalid manga id.");
        }

        var currentUser = await _userManager.GetUserAsync(User);
        manga.Followers.Remove(currentUser);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}