using System.Security.Claims;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Community;

public partial class CommentController
{
    [HttpGet("~/posts/{postId}/comments")]
    public async Task<IActionResult> GetPostComments(string postId, 
        [FromQuery] DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var comments = await _context.Comments
            .OfType<PostComment>()
            .Where(c => c.Post.Id == postId)
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

    [HttpPost("~/posts/{postId}/comments")]
    [Authorize]
    public async Task<IActionResult> PostCommentForPost(string postId,
        [FromForm] CommentEditDTO commentDTO)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
        {
            return NotFound("Post not found");
        }

        var comment = _mapper.Map<PostComment>(commentDTO);
        comment.User = await _userManager.GetUserAsync(User);
        comment.Post = post;

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<CommentDTO>(comment));
    }
}
