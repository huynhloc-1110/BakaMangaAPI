using AutoMapper;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("chapters")]
[ApiController]
public class ChapterController : ControllerBase
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

    [HttpGet("{id}/comments")]
    public async Task<IActionResult> GetCommentsForChapter(string id, [FromQuery]
        FilterDTO filter)
    {
        var commentCount = await _context.Comments
            .OfType<ChapterComment>()
            .Where(c => c.Chapter.Id == id && c.DeletedAt == null)
            .CountAsync();
        var comments = await _context.Comments
            .OfType<ChapterComment>()
            .Where(c => c.Chapter.Id == id && c.DeletedAt == null)
            .Include(c => c.User)
            .Include(c => c.ChildComments)
            .Include(c => c.Reacts)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        var commentList = _mapper.Map<List<CommentDTO>>(comments);

        // Check if the comment has react from current user
        if (User.Identity!.IsAuthenticated)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            for (int i = 0; i < comments.Count; i++)
            {
                var currentReact = comments[i].Reacts
                    .SingleOrDefault(r => r!.User == currentUser, null);
                commentList[i].UserReactFlag = (currentReact != null) ?
                    (int)currentReact.ReactFlag : 0;
            }
        }

        var paginatedCommentList = new PaginatedListDTO<CommentDTO>(
            commentList, commentCount, filter.Page, filter.PageSize);
        return Ok(paginatedCommentList);
    }

    [HttpPost("{id}/comments")]
    public async Task<IActionResult> PostCommentForChapter(string id,
        [FromForm] CommentEditDTO commentDTO)
    {
        var comment = _mapper.Map<ChapterComment>(commentDTO);
        comment.User = await _userManager.GetUserAsync(User);
        var chapter = await _context.Chapters.FindAsync(id);
        if (chapter == null)
        {
            return NotFound("Chapter not found");
        }
        comment.Chapter = chapter;

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<CommentDTO>(comment));
    }
}
