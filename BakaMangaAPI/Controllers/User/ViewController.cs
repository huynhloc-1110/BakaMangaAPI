using System.Security.Claims;

using BakaMangaAPI.Data;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BakaMangaAPI.Controllers;

[ApiController]
[Authorize]
public class ViewController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ViewController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost("~/chapters/{chapterId}/my-view")]
    [Authorize]
    public async Task<ActionResult> PostViewForChapter(string chapterId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var chapter = await _context.Chapters.FindAsync(chapterId);
        if (chapter == null)
        {
            return NotFound("Chapter not found");
        }

        ChapterView view = new() { ChapterId = chapterId, UserId = userId };
        _context.Views.Add(view);

        try
        {
            await _context.SaveChangesAsync();
            return Ok();
        } catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPost("~/posts/{postId}/my-view")]
    [Authorize]
    public async Task<ActionResult> PostViewForPost(string postId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
        {
            return NotFound("Chapter not found");
        }

        PostView view = new() { PostId = postId, UserId = userId };
        _context.Views.Add(view);

        try
        {
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}
