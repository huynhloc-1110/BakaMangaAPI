using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public partial class CommentController
{
    // GET: /mangas/5/comments
    [HttpGet("~/mangas/{id}/comments")]
    public async Task<IActionResult> GetMangaComments(string id, [FromQuery]
        FilterDTO filter)
    {
        var commentCount = await _context.MangaComments
            .Where(c => c.Manga.Id == id && c.DeletedAt == null)
            .CountAsync();
        var comments = await _context.MangaComments
            .Where(c => c.Manga.Id == id && c.DeletedAt == null)
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

    // POST: /mangas/3/comments/5
    [HttpPost("~/mangas/{id}/comments")]
    [Authorize]
    public async Task<IActionResult> PostCommentForManga(string id,
        [FromForm] CommentEditDTO commentDTO)
    {
        var comment = _mapper.Map<MangaComment>(commentDTO);
        comment.User = await _userManager.GetUserAsync(User);
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
}
