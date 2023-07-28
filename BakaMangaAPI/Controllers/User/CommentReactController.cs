using Microsoft.AspNetCore.Mvc;
using BakaMangaAPI.Data;
using Microsoft.AspNetCore.Identity;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers;

[Route("user/react/comments")]
[ApiController]
[Authorize]
public class CommentReactController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CommentReactController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    private async Task<CommentReact?> LoadReactAsync(ApplicationUser user,
        Comment comment)
    {
        return await _context.CommentReacts
            .Include(r => r.User)
            .Include(r => r.Comment)
            .SingleOrDefaultAsync(r => r.User == user && r.Comment == comment);
    }

    // GET: /user/react/comments/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserReactForComment(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return BadRequest();
        }
        var react = await LoadReactAsync(currentUser, comment);
        return Ok(react == null ? 0 : (int)react.ReactFlag);
    }

    // POST: /user/react/comments/5
    [HttpPost("{id}")]
    public async Task<IActionResult> PostUserReactForComment(string id, [FromForm]
        ReactFlag reactFlag)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return BadRequest("Invalid comment id");
        }

        var currentReact = await LoadReactAsync(currentUser, comment);
        if (currentReact != null)
        {
            return BadRequest("React already exists. Use PUT instead.");
        }

        var react = new CommentReact()
        {
            ReactFlag = reactFlag,
            User = currentUser,
            Comment = comment
        };
        _context.CommentReacts.Add(react);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserReactForComment), new { id },
            (int)reactFlag);
    }

    // PUT: /user/react/comments/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutReactForComment(string id, [FromForm]
        ReactFlag reactFlag)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return BadRequest("Invalid manga id or rating value.");
        }

        var currentReact = await LoadReactAsync(currentUser, comment);
        if (currentReact == null)
        {
            return NotFound("Rating not found.");
        }

        currentReact.ReactFlag = reactFlag;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: /user/react/comments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReactForComment(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return BadRequest("Invalid manga id.");
        }

        var currentReact = await LoadReactAsync(currentUser, comment);
        if (currentReact == null)
        {
            return BadRequest();
        }

        _context.CommentReacts.Remove(currentReact);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
