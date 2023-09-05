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

    [HttpGet("~/uploader/{uploaderId}/chapters")]
    public async Task<IActionResult> GetChaptersOfUploader(string uploaderId,
        [FromQuery] FilterDTO filter)
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
            query = query.Where(c => c.Manga.OriginalTitle.Contains(filter.Search));
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

    [HttpPost]
    public async Task<IActionResult> PostChapter([FromForm] ChapterEditDTO dto)
    {
        var manga = await _context.Mangas.FindAsync(dto.MangaId);
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
}
