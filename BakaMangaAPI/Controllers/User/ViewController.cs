using Microsoft.AspNetCore.Mvc;
using BakaMangaAPI.Data;
using Microsoft.AspNetCore.Identity;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers;

[Route("user/view/chapters")]
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

    [HttpPost("{id}")]
    [Authorize]
    public async Task<ActionResult> PostViewForChapter(string id)
    {
        var user = await _userManager.GetUserAsync(User);
        var chapter = await _context.Chapters.FindAsync(id);
        if (chapter == null)
        {
            return NotFound("Chapter not found");
        }

        ChapterView view = new()
        {
            Chapter = chapter,
            User = user
        };
        _context.Views.Add(view);

        try
        {
            await _context.SaveChangesAsync();
        } catch (Exception)
        {
            return BadRequest();
        }

        return Ok();
    }
}
