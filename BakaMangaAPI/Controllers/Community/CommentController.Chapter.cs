using System.Security.Claims;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Community;

public partial class CommentController
{
    [HttpGet("~/chapters/{chapterId}/comments")]
    public async Task<IActionResult> GetCommentsForChapter(string chapterId,
        [FromQuery] DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var comments = await _context.Comments
            .OfType<ChapterComment>()
            .Where(c => c.Chapter.Id == chapterId)
            .Where(c => createdAtCursor == null || c.CreatedAt < createdAtCursor)
            .OrderByDescending(p => p.CreatedAt)
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

    [HttpPost("~/chapters/{chapterId}/comments")]
    [Authorize]
    public async Task<IActionResult> PostCommentForChapter(string chapterId,
        [FromForm] CommentEditDTO commentDTO)
    {
        var chapter = await _context.Chapters.FindAsync(chapterId);
        if (chapter == null)
        {
            return NotFound("Chapter not found");
        }

        var comment = _mapper.Map<ChapterComment>(commentDTO);
        comment.User = await _userManager.GetUserAsync(User);
        comment.Chapter = chapter;

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<CommentDTO>(comment));
    }
}
