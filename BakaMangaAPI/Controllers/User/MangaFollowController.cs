using BakaMangaAPI.Data;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

[Route("user/follow/mangas")]
[ApiController]
[Authorize]
public class MangaFollowController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHubContext<ChatHub> _chatHub;

    public MangaFollowController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IHubContext<ChatHub> chatHub)
    {
        _context = context;
        _userManager = userManager;
        _chatHub = chatHub;
    }

    // GET: /user/follow/mangas/5
    [HttpGet("{id}")]
    public async Task<bool> GetUserFollowForManga(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var result = await _context.Mangas
            .AnyAsync(m => m.Id == id &&
                m.Followers.Any(u => u == currentUser));
        return result;
    }

    // POST: /user/follow/mangas/5
    [HttpPost("{id}")]
    public async Task<IActionResult> PostUserFollowForManga(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var manga = await _context.Mangas.FindAsync(id);

        if (manga == null)
        {
            return BadRequest("Invalid manga id.");
        }
        if (await GetUserFollowForManga(id))
        {
            return BadRequest("User has already followed this manga.");
        }

        manga.Followers.Add(currentUser);
        await _context.SaveChangesAsync();

        await _chatHub.Clients.All.SendAsync("ReceiveMessage", "admin", "You have followed");
        return CreatedAtAction(nameof(GetUserFollowForManga), new { id }, true);
    }

    // DELETE: /user/follow/mangas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveUserFollowForManga(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var manga = await _context.Mangas
            .Include(m => m.Followers)
            .SingleOrDefaultAsync(m => m.Id == id);

        if (manga == null)
        {
            return BadRequest("Invalid manga id.");
        }

        manga.Followers.Remove(currentUser);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
