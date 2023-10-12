using System.Security.Claims;
using AutoMapper.QueryableExtensions;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.User;

public partial class MangaListController
{
    [HttpGet("~/followed-manga-lists")]
    [Authorize]
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

    [HttpPost("~/user/follow/manga-lists/{mangaListId}")]
    [Authorize]
    public async Task<IActionResult> PostUserFollowForMangaList(string mangaListId)
    {
        if (await _context.MangaLists.FindAsync(mangaListId) is not MangaList mangaList)
        {
            return BadRequest("Manga list not found.");
        }
        if (await _userManager.GetUserAsync(User) is not ApplicationUser user)
        {
            return BadRequest("Token outdated or corrupted");
        }

        mangaList.Followers.Add(new MangaListFollower
        {
            User = user,
        });

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("~/user/follow/manga-lists/{mangaListId}")]
    [Authorize]
    public async Task<IActionResult> DeleteUserFollowForMangaList(string mangaListId)
    {
        if (await _context.MangaLists.FindAsync(mangaListId) is not MangaList mangaList)
        {
            return BadRequest("Manga list not found.");
        }
        if (await _userManager.GetUserAsync(User) is not ApplicationUser user)
        {
            return BadRequest("Token outdated or corrupted");
        }

        var mangaListFollower = _context.MangaListFollowers
            .SingleOrDefault(f => f.User == user && f.MangaList == mangaList);
        if (mangaListFollower != null)
        {
            _context.MangaListFollowers.Remove(mangaListFollower);
        }
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
