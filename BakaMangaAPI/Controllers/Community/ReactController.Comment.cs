using System.Security.Claims;

using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Community;

public partial class ReactController
{
    private async Task<CommentReact?> GetCommentReactAsync(string userId, string commentId)
    {
        return await _context.Reacts
            .OfType<CommentReact>()
            .SingleOrDefaultAsync(r => r.UserId == userId && r.CommentId == commentId);
    }

    [HttpGet("~/comments/{commentId}/my-react")]
    [Authorize]
    public async Task<IActionResult> GetMyReactForComment(string commentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var react = await GetCommentReactAsync(userId, commentId);
        return react == null ? NotFound() : Ok(react.ReactFlag);
    }

    [HttpPut("~/comments/{commentId}/my-react")]
    [Authorize]
    public async Task<IActionResult> PutMyReactForComment(string commentId, [FromForm]
        ReactFlag reactFlag)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentReact = await GetCommentReactAsync(userId, commentId);

        // react exists
        if (currentReact != null)
        {
            currentReact.ReactFlag = reactFlag;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // react not exists yet
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        var newReact = new CommentReact()
        {
            ReactFlag = reactFlag,
            UserId = userId,
            CommentId = commentId
        };

        _context.Reacts.Add(newReact);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMyReactForComment), new { commentId }, newReact.ReactFlag);
    }

    [HttpDelete("~/comments/{commentId}/my-react")]
    [Authorize]
    public async Task<IActionResult> DeleteReactForComment(string commentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var react = await GetCommentReactAsync(userId, commentId);
        if (react == null)
        {
            return NotFound();
        }

        _context.Reacts.Remove(react);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
