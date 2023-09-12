using BakaMangaAPI.DTOs;
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
            .Select(i => new
            {
                i.Manga,
                Chapters = i.Manga.Chapters.OrderByDescending(c => c.CreatedAt).Take(3).ToList(),
            })
            .ToListAsync();
        var mangasDTO = mangas
            .Select(m => new ChapterGroupingDTO
            {
                Manga = _mapper.Map<MangaBasicDTO>(m.Manga),
                Chapters = _mapper.Map<List<ChapterBasicDTO>>(m.Chapters)
            })
            .ToList();

        return Ok(mangasDTO);
    }
}
