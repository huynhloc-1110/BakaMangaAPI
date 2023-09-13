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
    public async Task<IActionResult> GetFollowedMangaLists()
    {
        if (await _userManager.GetUserAsync(User) is not ApplicationUser user)
        {
            return BadRequest("Token outdated or corrupted");
        }

        var mangaLists = await _context.MangaLists
            .Where(ml => ml.Followers.Select(f => f.User).Contains(user))
            .Where(ml => ml.Type == MangaListType.Public)
            .OrderByDescending(ml => ml.Followers.Single(f => f.User == user).FollowedAt)
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
                UpdatedAt = ml.Items.Any() ? ml.Items.Max(i => i.AddedAt) : ml.CreatedAt
            })
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
