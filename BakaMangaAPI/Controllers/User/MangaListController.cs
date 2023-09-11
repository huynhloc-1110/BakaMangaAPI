using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.User;

[Route("manga-lists")]
[ApiController]
public class MangaListController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MangaListController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("~/users/{userId}/manga-lists")]
    public async Task<IActionResult> GetUserMangaLists(string userId)
    {
        var mangaListQuery = _context.MangaLists
            .Where(ml => ml.Owner.Id == userId);

        if (User.Identity!.IsAuthenticated != true ||
            await _userManager.GetUserAsync(User) is not ApplicationUser user ||
            user.Id != userId)
        {
            mangaListQuery = mangaListQuery.Where(ml => ml.Type == MangaListType.Public);
        }

        var mangaLists = await mangaListQuery
            .Select(ml => new MangaListBasicDTO
            {
                Id = ml.Id,
                Name = ml.Name,
                Type = ml.Type,
                MangaCoverUrls = ml.Mangas
                    .Where(m => m.DeletedAt == null)
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(3)
                    .Select(m => m.CoverPath)
                    .ToList()
            })
            .AsNoTracking()
            .ToListAsync();

        return Ok(mangaLists);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostMangaList([FromForm] MangaListEditDTO dto)
    {
        if (await _userManager.GetUserAsync(User) is not ApplicationUser user)
        {
            return BadRequest("Token outdated or corrupted");
        }

        var mangaList = new MangaList
        {
            Name = dto.Name,
            Type = dto.Type,
            Owner = user
        };

        _context.MangaLists.Add(mangaList);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("{mangaListId}")]
    [Authorize]
    public async Task<IActionResult> PutMangaList(string mangaListId, [FromForm] MangaListEditDTO dto)
    {
        var mangaList = await _context.MangaLists
            .Include(ml => ml.Mangas)
            .SingleOrDefaultAsync(ml => ml.Id == mangaListId);
        if (mangaList == null)
        {
            return BadRequest("Manga list not found");
        }

        mangaList.Name = dto.Name;
        mangaList.Type = dto.Type;
        if (!string.IsNullOrEmpty(dto.AddedMangaId) &&
            await _context.Mangas.FindAsync(dto.AddedMangaId) is Manga addedManga)
        {
            mangaList.Mangas.Add(addedManga);
        }
        if (!string.IsNullOrEmpty(dto.RemovedMangaId) &&
            await _context.Mangas.FindAsync(dto.RemovedMangaId) is Manga removedManga)
        {
            mangaList.Mangas.Remove(removedManga);
        }
        
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{mangaListId}")]
    public async Task<IActionResult> DeleteMangaList(string mangaListId)
    {
        if (await _context.MangaLists.FindAsync(mangaListId) is not MangaList mangaList)
        {
            return BadRequest("Manga list not found");
        }

        _context.MangaLists.Remove(mangaList);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
