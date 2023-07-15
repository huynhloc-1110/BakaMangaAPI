using Microsoft.AspNetCore.Mvc;
using BakaMangaAPI.Data;
using Microsoft.AspNetCore.Identity;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers;

[Route("user/[controller]")]
[ApiController]
[Authorize]
public class RatingController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public RatingController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    private async Task<Rating?> LoadRatingAsync(ApplicationUser user, Manga manga)
    {
        var rating = await _context.Ratings
            .Include(r => r.User)
            .Include(r => r.Manga)
            .SingleOrDefaultAsync(r => r.User == user && r.Manga == manga);
        return rating;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserRatingForManga(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var manga = await _context.Mangas.FindAsync(id);
        if (manga == null)
        {
            return BadRequest();
        }
        return Ok((await LoadRatingAsync(currentUser, manga))?.Value);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> PostRatingForManga(string id, [FromForm]
        int inputRating)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var manga = await _context.Mangas.FindAsync(id);
        if (manga == null)
        {
            return BadRequest();
        }

        var rating = new Rating()
        {
            Value = inputRating,
            User = currentUser,
            Manga = manga
        };
        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUserRatingForManga", new { id }, inputRating);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutRatingForManga(string id, [FromForm]
        int inputRating)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var manga = await _context.Mangas.FindAsync(id);
        if (manga == null)
        {
            return BadRequest();
        }
        var rating = await LoadRatingAsync(currentUser, manga);
        if (rating == null)
        {
            return BadRequest();
        }

        rating.Value = inputRating;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRatingForManga(string id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var manga = await _context.Mangas.FindAsync(id);
        if (manga == null)
        {
            return BadRequest();
        }
        var rating = await LoadRatingAsync(currentUser, manga);
        if (rating == null)
        {
            return BadRequest();
        }
        _context.Ratings.Remove(rating);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
