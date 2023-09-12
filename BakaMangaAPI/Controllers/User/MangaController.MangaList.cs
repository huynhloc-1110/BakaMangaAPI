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
            .Include(i => i.Manga)
                .ThenInclude(m => m.Chapters)
                    .ThenInclude(c => c.ChapterViews)
            .Include(i => i.Manga)
                .ThenInclude(m => m.Chapters)
                    .ThenInclude(c => c.Uploader)
            .Include(i => i.Manga)
                .ThenInclude(m => m.Chapters)
                    .ThenInclude(c => c.UploadingGroup)
            .Where(i => i.MangaListId == mangaListId)
            .OrderBy(i => i.Index)
            .Select(i => new ChapterGroupingDTO
            {
                Manga = _mapper.Map<MangaBasicDTO>(i.Manga),
                Chapters = _mapper.Map<List<ChapterBasicDTO>>(i.Manga.Chapters
                    .OrderBy(c => c.CreatedAt).Take(3).ToList())
            })
            .ToListAsync();

        return Ok(mangas);
    }
}
