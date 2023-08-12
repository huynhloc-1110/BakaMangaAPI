using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public partial class CommentController
{
    // GET: /comments/5/children
    [HttpGet("{id}/children")]
    public async Task<IActionResult> GetChildComments(string id,
        [FromQuery] FilterDTO filter)
    {
        var commentCount = await _context.Comments
            .Where(c => c.ParentComment!.Id == id && c.DeletedAt == null)
            .CountAsync();
        var comments = await _context.Comments
            .Where(c => c.ParentComment!.Id == id && c.DeletedAt == null)
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
                    .SingleOrDefault(r => r!.User == currentUser, null);
                commentList[i].UserReactFlag = (currentReact != null) ?
                    (int)currentReact.ReactFlag : 0;
            }
        }

        var paginatedCommentList = new PaginatedListDTO<CommentDTO>(
            commentList, commentCount, filter.Page, filter.PageSize);
        return Ok(paginatedCommentList);
    }

    // POST: /comments/5/children
    [HttpPost("{id}/children")]
    [Authorize]
    public async Task<IActionResult> PostChildCommentFor(string id,
        [FromForm] CommentEditDTO commentDTO)
    {
        var parentComment = await _context.Comments.FindAsync(id);
        if (parentComment == null)
        {
            return NotFound("Parent comment not found");
        }

        var comment = _mapper.Map<Comment>(commentDTO);
        comment.User = await _userManager.GetUserAsync(User);
        comment.ParentComment = parentComment;

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<CommentDTO>(comment));
    }
}
