using BakaMangaAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public partial class ChapterController
{
    // GET: /mangas/5/chapters
    [HttpGet("~/mangas/{id}/chapters")]
    public async Task<IActionResult> GetChaptersForManga(string id, [FromQuery]
        FilterDTO filter)
    {
        // filter search
        var query = _context.Chapters.Where(c => c.DeletedAt == null);
        if (!string.IsNullOrEmpty(filter.Search))
        {
            if (int.TryParse(filter.Search, out int searchNum))
            {
                query = query.Where(c => c.Number == searchNum ||
                    c.Name.Contains(filter.Search));
            }
            else
            {
                query = query.Where(c => c.Name.ToLower()
                    .Contains(filter.Search.ToLower()));
            }
        }

        // filter manga
        var manga = await _context.Mangas.FindAsync(id);
        query = query
            .Include(c => c.Manga)
            .Where(c => c.Manga == manga);

        // count total chapter numbers
        var chapterNumberCount = await query
            .Select(c => c.Number)
            .Distinct()
            .CountAsync();

        // get chapters based on filter.Page and PageSize
        var chapterNumberFilter = query
            .Select(c => c.Number)
            .Distinct()
            .OrderByDescending(cn => cn)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize);
        var chapters = await query
            .Where(c => chapterNumberFilter.Contains(c.Number))
            .Include(c => c.Uploader)
            .Include(c => c.ChapterViews)
            .Include(c => c.UploadingGroup)
            .OrderByDescending(c => c.Number)
                .ThenBy(c => c.Language)
                    .ThenByDescending(c => c.CreatedAt)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        if (chapters.Count == 0)
        {
            return NotFound();
        }

        var chapterList = _mapper.Map<List<ChapterBasicDTO>>(chapters);
        var paginatedChapterList = new PaginatedListDTO<ChapterBasicDTO>
            (chapterList, chapterNumberCount, filter.Page, filter.PageSize);
        return Ok(paginatedChapterList);
    }
}
