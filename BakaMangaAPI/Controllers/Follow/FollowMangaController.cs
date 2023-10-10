using System.Security.Claims;

using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var mangaQuery = _context.Mangas
            .Where(m => m.Followers.Select(f => f.Id).Contains(currentUserId));

        var mangaCount = await mangaQuery.CountAsync();

        var mangas = await mangaQuery
            .ProjectTo<MangaBlockDTO>(_mapper.ConfigurationProvider, new { currentUserId })
            .OrderByDescending(g => g.UpdatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsNoTracking()
            .ToListAsync();

        var paginatedList = new PaginatedListDTO<MangaBlockDTO>
            (mangas, mangaCount, filter.Page, filter.PageSize);
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