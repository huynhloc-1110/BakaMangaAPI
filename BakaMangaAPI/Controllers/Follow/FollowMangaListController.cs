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

[ApiController]
[Authorize]
[Route("manga-lists/{mangaListId}/my-follow")]
public class FollowMangaListController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public FollowMangaListController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet("~/followed-manga-lists")]
    public async Task<IActionResult> GetFollowedMangaLists([FromQuery] DateTime? updatedAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var mangaLists = await _context.MangaLists
            .Where(ml => ml.Followers.Select(f => f.UserId).Contains(currentUserId))
            .Where(ml => ml.Type == MangaListType.Public)
            .ProjectTo<MangaListBasicDTO>(_mapper.ConfigurationProvider, new { updatedAtCursor })
            .Where(ml => updatedAtCursor == null || ml.UpdatedAt < updatedAtCursor)
            .OrderByDescending(ml => ml.UpdatedAt)
            .Take(4)
            .AsNoTracking()
            .ToListAsync();

        return Ok(mangaLists);
    }

    [HttpPost]
    public async Task<IActionResult> PostUserFollowForMangaList(string mangaListId)
    {
        if (await _context.MangaLists.FindAsync(mangaListId) is not MangaList mangaList)
        {
            return NotFound("Manga list not found.");
        }

        mangaList.Followers.Add(new MangaListFollower
        {
            User = await _userManager.GetUserAsync(User),
        });

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUserFollowForMangaList(string mangaListId)
    {
        if (await _context.MangaLists.FindAsync(mangaListId) is not MangaList mangaList)
        {
            return NotFound("Manga list not found.");
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var mangaListFollower = _context.MangaListFollowers
            .SingleOrDefault(f => f.UserId == currentUserId && f.MangaList == mangaList);
        if (mangaListFollower != null)
        {
            _context.MangaListFollowers.Remove(mangaListFollower);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}