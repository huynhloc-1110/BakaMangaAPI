using AutoMapper;
using AutoMapper.QueryableExtensions;

using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

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

    [HttpGet("{chapterId}")]
    public async Task<IActionResult> GetChapter(string chapterId)
    {
        var chapter = await _context.Chapters
            .ProjectTo<ChapterDetailDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .SingleOrDefaultAsync(ch => ch.Id == chapterId);

        return (chapter != null) ? Ok(chapter) : NotFound();
    }

    [HttpGet("{chapterId}/related-chapters")]
    public async Task<IActionResult> GetRelatedChapters(string chapterId)
    {
        var chapter = await _context.Chapters
            .Include(ch => ch.Manga)
            .SingleOrDefaultAsync(ch => ch.Id == chapterId);
        if (chapter == null)
        {
            return NotFound();
        }

        var relatedChapters = await _context.Chapters
            .Where(ch => ch.Manga == chapter.Manga)
            .Where(ch => ch.Language == chapter.Language)
            .OrderByDescending(ch => ch.Number)
            .ProjectTo<ChapterSimpleDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        return Ok(relatedChapters);
    }
}
