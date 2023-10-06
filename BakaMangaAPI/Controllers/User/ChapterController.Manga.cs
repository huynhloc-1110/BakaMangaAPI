using System.Security.Claims;

using AutoMapper.QueryableExtensions;
using BakaMangaAPI.DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public partial class ChapterController
{
    [HttpGet("~/mangas/{mangaId}/chapters")]
    public async Task<IActionResult> GetChaptersForManga(string mangaId, [FromQuery]
        FilterDTO filter)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = _context.Chapters.Where(
            c => c.DeletedAt == null && c.Manga.Id == mangaId);

        // filter search
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
            .OrderByDescending(c => c.Number)
                .ThenBy(c => c.Language)
                    .ThenByDescending(c => c.CreatedAt)
            .ProjectTo<ChapterBasicDTO>(_mapper.ConfigurationProvider, new { currentUserId })
            .AsNoTracking()
            .ToListAsync();

        var paginatedChapterList = new PaginatedListDTO<ChapterBasicDTO>
            (chapters, chapterNumberCount, filter.Page, filter.PageSize);
        return Ok(paginatedChapterList);
    }
}
