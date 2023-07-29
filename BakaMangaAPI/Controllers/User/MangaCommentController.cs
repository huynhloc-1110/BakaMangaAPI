using System.Security.Claims;
using AutoMapper;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Authorize]
public class MangaCommentController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public MangaCommentController(ApplicationDbContext context, IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    // GET: /manga-comments/5/children
    [AllowAnonymous]
    [HttpGet("manga-comments/{id}/children")]
    public async Task<IActionResult> LoadMangaChildComments(string id,
        [FromQuery] FilterDTO filter)
    {
        var commentCount = await _context.MangaComments
            .Where(c => c.ParentComment!.Id == id)
            .CountAsync();
        var comments = await _context.MangaComments
            .Where(c => c.ParentComment!.Id == id)
            .Include(c => c.User)
            .Include(c => c.ChildComments)
            .Include(c => c.Reacts)
            .OrderBy(c => c.CreatedAt)
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
                    .SingleOrDefault(r => r!.UserId == currentUser.Id, null);
                commentList[i].UserReactFlag = (currentReact != null) ?
                    (int)currentReact.ReactFlag : 0;
            }
        }

        var paginatedCommentList = new PaginatedListDTO<CommentDTO>(
            commentList, commentCount, filter.Page, filter.PageSize);
        return Ok(paginatedCommentList);
    }

    [HttpPost("mangas/{id}/comments")]
    [Authorize]
    public async Task<IActionResult> PostCommentForManga(string id,
        [FromForm] CommentEditDTO commentDTO)
    {
        var comment = _mapper.Map<MangaComment>(commentDTO);
        comment.UserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var manga = await _context.Mangas.FindAsync(id);
        if (manga == null)
        {
            return NotFound("Manga not found");
        }
        comment.Manga = manga;

        _context.MangaComments.Add(comment);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<CommentDTO>(comment));
    }

    [HttpPut("manga-comments/{id}")]
    [Authorize]
    public async Task<IActionResult> PutMangaComment(string id,
        [FromForm] CommentEditDTO commentDTO)
    {
        if (id != commentDTO.Id)
        {
            return BadRequest("Comment Id not matched");
        }

        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        if (!IsUserComment(User, comment))
        {
            return BadRequest("The comment is not owned by the current user");
        }

        comment = _mapper.Map(commentDTO, comment);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<CommentDTO>(comment));
    }

    [HttpDelete("manga-comments/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteMangaComment(string id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        if (!IsUserComment(User, comment))
        {
            return BadRequest("The comment is not owned by the current user");
        }

        comment.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool IsUserComment(ClaimsPrincipal principal, Comment comment)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (comment.UserId == userId)
        {
            return true;
        }
        return false;
    }
}
