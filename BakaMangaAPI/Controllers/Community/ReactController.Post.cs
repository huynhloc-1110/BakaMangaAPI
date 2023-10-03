using System.Security.Claims;

using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Community;

public partial class ReactController
{
    private async Task<PostReact?> GetPostReactAsync(string userId, string postId)
    {
        return await _context.Reacts
            .OfType<PostReact>()
            .SingleOrDefaultAsync(r => r.User.Id == userId && r.Post.Id == postId);
    }

    [HttpGet("~/posts/{postId}/my-react")]
    [Authorize]
    public async Task<IActionResult> GetMyReactForPost(string postId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var react = await GetPostReactAsync(userId, postId);
        return react == null ? NotFound() : Ok(react.ReactFlag);
    }

    [HttpPut("~/posts/{postId}/my-react")]
    [Authorize]
    public async Task<IActionResult> PutMyReactForPost(string postId, [FromForm]
        ReactFlag reactFlag)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentReact = await GetPostReactAsync(userId, postId);

        // react exists
        if (currentReact != null)
        {
            currentReact.ReactFlag = reactFlag;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // react not exists yet
        var newReact = new PostReact()
        {
            ReactFlag = reactFlag,
            UserId = userId,
            PostId = postId
        };

        _context.Reacts.Add(newReact);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMyReactForPost), new { postId }, newReact.ReactFlag);
    }

    [HttpDelete("~/posts/{postId}/my-react")]
    [Authorize]
    public async Task<IActionResult> DeleteReactForPost(string postId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var react = await GetPostReactAsync(userId, postId);
        if (react == null)
        {
            return NotFound();
        }

        _context.Reacts.Remove(react);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
