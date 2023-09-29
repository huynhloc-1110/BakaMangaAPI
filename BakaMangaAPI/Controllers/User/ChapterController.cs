using AutoMapper;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("chapters")]
[ApiController]
public partial class ChapterController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public ChapterController(ApplicationDbContext context, IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChapter(string id)
    {
        var chapter = await _context.Chapters
            .Include(ch => ch.Images)
            .Include(ch => ch.Manga)
            .Include(ch => ch.UploadingGroup)
            .AsNoTracking()
            .SingleOrDefaultAsync(ch => ch.Id == id);

        if (chapter == null || chapter.DeletedAt != null)
        {
            return NotFound("The chapter of this id does not exist");
        }

        return Ok(_mapper.Map<ChapterDetailDTO>(chapter));
    }

    [HttpGet("{id}/related-chapters")]
    public async Task<IActionResult> GetRelatedChapters(string id)
    {
        var chapter = await _context.Chapters
            .Include(ch => ch.Manga)
            .SingleOrDefaultAsync(ch => ch.Id == id);

        if (chapter == null || chapter.DeletedAt != null)
        {
            return NotFound("The chapter of this id does not exist");
        }

        var relatedChapters = await _context.Chapters
            .Where(ch => ch.Manga == chapter.Manga)
            .Where(ch => ch.Language == chapter.Language)
            .Include(ch => ch.UploadingGroup)
            .OrderByDescending(ch => ch.Number)
            .AsNoTracking()
            .ToListAsync();

        return Ok(_mapper.Map<List<ChapterSimpleDTO>>(relatedChapters));
    }
}
