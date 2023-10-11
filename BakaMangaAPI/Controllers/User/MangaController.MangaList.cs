using System.Security.Claims;

using AutoMapper.QueryableExtensions;

using BakaMangaAPI.DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers;

public partial class MangaController
{
    [HttpGet("~/manga-lists/{mangaListId}/mangas/")]
    public async Task<IActionResult> GetMangasFromList(string mangaListId,
        [FromQuery] DateTime? updatedAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var mangas = await _context.MangaListItems
            .Where(i => i.MangaListId == mangaListId)
            .OrderBy(i => i.Index)
            .Select(i => i.Manga)
            .ProjectTo<MangaBlockDTO>(_mapper.ConfigurationProvider, new { currentUserId })
            .Where(g => updatedAtCursor == null || g.UpdatedAt < updatedAtCursor)
            .Take(4)
            .AsNoTracking()
            .ToListAsync();

        return Ok(mangas);
    }
}
