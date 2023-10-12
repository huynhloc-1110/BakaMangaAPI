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

namespace BakaMangaAPI.Controllers.User;

[Route("manga-lists")]
[ApiController]
public partial class MangaListController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public MangaListController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet("~/users/{userId}/manga-lists")]
    public async Task<IActionResult> GetUserMangaLists(string userId,
        [FromQuery] string? checkedMangaId,
        [FromQuery] DateTime? updatedAtCursor)
    {
        var mangaListQuery = _context.MangaLists
            .Where(ml => ml.Owner.Id == userId);

        // get only public list if the user is not owner
        if (User.Identity!.IsAuthenticated != true ||
            await _userManager.GetUserAsync(User) is not ApplicationUser user ||
            user.Id != userId)
        {
            mangaListQuery = mangaListQuery.Where(ml => ml.Type == MangaListType.Public);
        }

        var mangaLists = await mangaListQuery
            .ProjectTo<MangaListBasicDTO>(_mapper.ConfigurationProvider, new { checkedMangaId })
            .Where(ml => updatedAtCursor == null || ml.UpdatedAt < updatedAtCursor)
            .OrderByDescending(ml => ml.UpdatedAt)
            .Take(4)
            .AsNoTracking()
            .ToListAsync();

        return Ok(mangaLists);
    }

    [HttpGet("{mangaListId}")]
    public async Task<IActionResult> GetMangaList(string mangaListId)
    {
        string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var mangaList = await _context.MangaLists
            .Where(ml => ml.Id == mangaListId)
            .ProjectTo<MangaListBasicDTO>(_mapper.ConfigurationProvider, new { currentUserId })
            .OrderByDescending(ml => ml.UpdatedAt)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        return Ok(mangaList);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostMangaList([FromForm] MangaListEditDTO dto)
    {
        var mangaList = _mapper.Map<MangaList>(dto);
        mangaList.Owner = await _userManager.GetUserAsync(User);

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

        // validate
        if (mangaList == null)
        {
            return NotFound("Manga list not found");
        }
        if (mangaList.Owner != await _userManager.GetUserAsync(User))
        {
            return Forbid();
        }

        mangaList = _mapper.Map(dto, mangaList);

        // handle add, remove manga from list
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

        // validate
        if (mangaList == null)
        {
            return NotFound("Manga list not found");
        }
        if (mangaList.Owner != await _userManager.GetUserAsync(User))
        {
            return Forbid();
        }

        _context.MangaLists.Remove(mangaList);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
