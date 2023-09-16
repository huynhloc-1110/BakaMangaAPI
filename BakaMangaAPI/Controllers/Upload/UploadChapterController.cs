using AutoMapper;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers;

[ApiController]
[Authorize(Roles = "Uploader")]
[Route("chapters")]
public class UploadChapterController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMediaManager _mediaManager;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public UploadChapterController(ApplicationDbContext context, IMediaManager mediaManager,
        IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _mediaManager = mediaManager;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet("~/uploader/me/chapters")]
    public async Task<IActionResult> GetChaptersOfUploader([FromQuery] FilterDTO filter)
    {
        if (await _userManager.GetUserAsync(User) is not ApplicationUser uploader)
        {
            return BadRequest("Token outdated or corrupted.");
        }

        var query = _context.Chapters.Where(c => c.Uploader == uploader);

        // filter exclude deleted
        if (filter.ExcludeDeleted)
        {
            query = query.Where(c => c.DeletedAt == null);
        }

        // filter search
        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(c => c.Manga.OriginalTitle.ToLower()
                .Contains(filter.Search.ToLower()));
        }

        // pagination
        var chapterCount = await query.CountAsync();
        var chapters = await query
            .OrderByDescending(c => c.CreatedAt)
            .Include(c => c.Manga)
            .Include(c => c.UploadingGroup)
            .Include(c => c.Pages)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsNoTracking()
            .ToListAsync();
        if (chapters.Count == 0)
        {
            return NotFound();
        }

        var chapterList = _mapper.Map<List<ChapterDetailDTO>>(chapters);
        var paginatedChapterList = new PaginatedListDTO<ChapterDetailDTO>
            (chapterList, chapterCount, filter.Page, filter.PageSize);
        return Ok(paginatedChapterList);
    }

    [HttpGet("~/uploader/{uploaderId}/chaptersByManga")]
    [AllowAnonymous]
    public async Task<IActionResult> GetChaptersOfUploaderByManga(string uploaderId,
        [FromQuery] FilterDTO filter)
    {
        if (await _userManager.FindByIdAsync(uploaderId) is not ApplicationUser uploader)
        {
            return NotFound("User not found");
        }

        var chapterGroupingCount = await _context.Mangas
            .Where(m => m.Chapters.Select(c => c.Uploader).Contains(uploader))
            .CountAsync();

        var chapterGroupings = await _context.Chapters
            .Include(c => c.Uploader)
            .Include(c => c.UploadingGroup)
            .Include(c => c.ChapterViews)
            .Where(c => c.DeletedAt == null)
            .Where(c => c.Uploader == uploader)
            .GroupBy(c => c.Manga.Id)
            .Select(g => new
            {
                Manga = _context.Mangas.SingleOrDefault(m => m.Id == g.Key),
                Chapters = g.OrderByDescending(c => c.CreatedAt).Take(3).ToList(),
                UpdatedAt = g.Max(c => c.CreatedAt)
            })
            .OrderByDescending(g => g.UpdatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        var chapterGroupingList = chapterGroupings
            .Select(g => new ChapterGroupingDTO()
            {
                Manga = _mapper.Map<MangaSimpleDTO>(g.Manga),
                Chapters = _mapper.Map<List<ChapterBasicDTO>>(g.Chapters)
            })
            .ToList();
        var paginatedList = new PaginatedListDTO<ChapterGroupingDTO>
            (chapterGroupingList, chapterGroupingCount, filter.Page, filter.PageSize);

        return Ok(paginatedList);
    }

    [HttpPost("~/mangas/{mangaId}/chapters")]
    public async Task<IActionResult> PostChapter(string mangaId, [FromForm] ChapterEditDTO dto)
    {
        var manga = await _context.Mangas.FindAsync(mangaId);
        if (manga == null)
        {
            return BadRequest("Manga not found");
        }
        var group = await _context.Groups.FindAsync(dto.UploadingGroupId);
        if (group == null)
        {
            return BadRequest("Group not found");
        }
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest("JWT token outdated or corrupted");
        }

        var chapter = new Chapter()
        {
            Number = dto.Number,
            Name = dto.Name,
            Language = dto.Language,
            Manga = manga,
            UploadingGroup = group,
            Uploader = user
        };

        for (int i = 0; i < dto.Pages.Count; i++)
        {
            var page = dto.Pages[i];
            var pageId = Guid.NewGuid().ToString();
            var pagePath = await _mediaManager.UploadImageAsync(page, pageId, ImageType.Page);
            chapter.Pages.Add(new() { Id = pageId, Number = i, Path = pagePath });
        }

        _context.Chapters.Add(chapter);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<ChapterDetailDTO>(chapter));
    }

    [HttpPut("{chapterId}")]
    public async Task<IActionResult> PutChapter(string chapterId, [FromForm] ChapterEditDTO dto)
    {
        // get chapter with additional info
        var chapter = await _context.Chapters
            .Include(c => c.UploadingGroup)
            .Include(c => c.Pages)
            .SingleOrDefaultAsync(c => c.Id == chapterId);
        if (chapter == null)
        {
            return BadRequest("Chapter not found");
        }

        // update chapter basic info
        chapter.Number = dto.Number;
        chapter.Name = dto.Name;
        chapter.Language = dto.Language;

        // update chapter uploading group
        var uploadingGroup = await _context.Groups.FindAsync(dto.UploadingGroupId);
        if (uploadingGroup == null)
        {
            return BadRequest("Uploading group not found");
        }
        chapter.UploadingGroup = uploadingGroup;

        // update chapter images
        // delete old images
        foreach (var page in chapter.Pages)
        {
            await _mediaManager.DeleteImageAsync(page.Path);
        }

        _context.Pages.RemoveRange(chapter.Pages);
        chapter.Pages = new();

        // upload new images
        for (int i = 0; i < dto.Pages.Count; i++)
        {
            var page = dto.Pages[i];
            var pageId = Guid.NewGuid().ToString();
            var pagePath = await _mediaManager.UploadImageAsync(page, pageId, ImageType.Page);
            chapter.Pages.Add(new() { Id = pageId, Number = i, Path = pagePath });
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{chapterId}")]
    public async Task<IActionResult> DeleteChapter(string chapterId, [FromQuery] bool undelete)
    {
        if (await _context.Chapters.FindAsync(chapterId) is not Chapter chapter)
        {
            return BadRequest("Chapter not found");
        }
        chapter.DeletedAt = undelete ? null : DateTime.UtcNow;
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}
