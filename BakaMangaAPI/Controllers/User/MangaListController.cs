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
public partial class MangaListController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MangaListController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("~/users/{userId}/manga-lists")]
    public async Task<IActionResult> GetUserMangaLists(string userId,
        [FromQuery] string? checkedMangaId)
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
                Owner = new UserSimpleDTO { Id = ml.Owner.Id, Name = ml.Owner.Name },
                MangaCoverUrls = ml.Items
                    .OrderBy(i => i.Index)
                    .Select(i => i.Manga)
                    .Where(m => m.DeletedAt == null)
                    .Take(3)
                    .Select(m => m.CoverPath)
                    .ToList(),
                UpdatedAt = ml.Items.Any() ? ml.Items.Max(i => i.AddedAt) : ml.CreatedAt,
                AlreadyAdded = !string.IsNullOrEmpty(checkedMangaId) &&
                    ml.Items.Select(i => i.MangaId).Contains(checkedMangaId)
            })
            .OrderByDescending(ml => ml.UpdatedAt)
            .AsNoTracking()
            .ToListAsync();

        return Ok(mangaLists);
    }

    [HttpGet("{mangaListId}")]
    public async Task<IActionResult> GetMangaList(string mangaListId)
    {
        var mangaList = await _context.MangaLists
            .Where(ml => ml.Id == mangaListId)
            .Select(ml => new MangaListBasicDTO
            {
                Id = ml.Id,
                Name = ml.Name,
                Type = ml.Type,
                Owner = new UserSimpleDTO { Id = ml.Owner.Id, Name = ml.Owner.Name },
                MangaCoverUrls = ml.Items
                    .OrderBy(i => i.Index)
                    .Select(i => i.Manga)
                    .Where(m => m.DeletedAt == null)
                    .Take(3)
                    .Select(m => m.CoverPath)
                    .ToList(),
                UpdatedAt = ml.Items.Any() ? ml.Items.Max(i => i.AddedAt) : ml.CreatedAt,
            })
            .OrderByDescending(ml => ml.UpdatedAt)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        return Ok(mangaList);
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
            .Include(ml => ml.Owner)
            .Include(ml => ml.Items)
                .ThenInclude(i => i.Manga)
            .SingleOrDefaultAsync(ml => ml.Id == mangaListId);

        if (mangaList == null)
        {
            return BadRequest("Manga list not found");
        }
        if (mangaList.Owner != await _userManager.GetUserAsync(User))
        {
            return BadRequest("Current user is not the owner of this manga list");
        }

        mangaList.Name = dto.Name;
        mangaList.Type = dto.Type;
        if (!string.IsNullOrEmpty(dto.AddedMangaId) &&
            await _context.Mangas.FindAsync(dto.AddedMangaId) is Manga addedManga)
        {
            mangaList.Items.Add(new() { Manga = addedManga, Index = mangaList.Items.Count });
        }
        if (!string.IsNullOrEmpty(dto.RemovedMangaId))
        {
            int removedIndex = mangaList.Items.FindIndex(i => i.Manga.Id == dto.RemovedMangaId);
            if (removedIndex != -1)
            {
                mangaList.Items.RemoveAt(removedIndex);
            }
        }
        
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{mangaListId}")]
    public async Task<IActionResult> DeleteMangaList(string mangaListId)
    {
        var mangaList = await _context.MangaLists
            .Include(ml => ml.Owner)
            .SingleOrDefaultAsync(ml => ml.Id == mangaListId);

        if (mangaList == null)
        {
            return BadRequest("Manga list not found");
        }
        if (mangaList.Owner != await _userManager.GetUserAsync(User))
        {
            return BadRequest("Current user is not the owner of this manga list");
        }

        _context.MangaLists.Remove(mangaList);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
