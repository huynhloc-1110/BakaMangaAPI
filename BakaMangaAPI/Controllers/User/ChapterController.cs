using AutoMapper;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("chapters")]
[ApiController]
public class ChapterController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ChapterController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChapter(string id)
    {
        var chapter = await _context.Chapters
            .Include(ch => ch.Pages)
            .Include(ch => ch.Manga)
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
            .OrderByDescending(ch => ch.Number)
            .AsNoTracking()
            .ToListAsync();

        return Ok(_mapper.Map<List<ChapterSimpleDTO>>(relatedChapters));
    }
}