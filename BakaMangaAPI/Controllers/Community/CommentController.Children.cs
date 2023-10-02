using System.Security.Claims;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Community;

public partial class CommentController
{
    [HttpGet("{commentId}/children")]
    public async Task<IActionResult> GetChildComments(string commentId,
        [FromQuery] DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var comments = await _context.Comments
            .Where(c => c.ParentComment!.Id == commentId)
            .Where(c => createdAtCursor == null || c.CreatedAt > createdAtCursor)
            .OrderBy(p => p.CreatedAt)
            .Take(4)
            .Select(c => new CommentDTO
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                User = new UserSimpleDTO
                {
                    Id = c.User.Id,
                    Name = c.User.Name,
                    AvatarPath = c.User.AvatarPath
                },
                LikeCount = c.Reacts.Count(r => r.ReactFlag == ReactFlag.Like),
                DislikeCount = c.Reacts.Count(r => r.ReactFlag == ReactFlag.Dislike),
                ChildCommentCount = c.ChildComments.Count,
                UserReactFlag = c.Reacts
                    .Where(r => r.UserId == currentUserId)
                    .Select(r => r.ReactFlag)
                    .SingleOrDefault()
            })
            .ToListAsync();

        return Ok(comments);
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
