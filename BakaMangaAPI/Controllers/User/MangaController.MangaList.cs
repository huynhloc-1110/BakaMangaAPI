using AutoMapper.QueryableExtensions;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers;

public partial class MangaController
{
    [HttpGet("~/manga-lists/{mangaListId}/mangas/")]
    public async Task<IActionResult> GetMangasFromList(string mangaListId)
    {
        var mangas = await _context.MangaListItems
            .Where(i => i.MangaListId == mangaListId)
            .OrderBy(i => i.Index)
            .ProjectTo<ChapterGroupingDTO>(_mapper.ConfigurationProvider)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        return Ok(mangas);
    }
}
