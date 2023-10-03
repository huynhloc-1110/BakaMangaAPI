using System.Security.Claims;

using BakaMangaAPI.Data;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers;

[Route("mangas/{mangaId}/my-rating")]
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

    [HttpGet]
    public async Task<IActionResult> GetMyRatingForManga(string mangaId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var rating = await _context.Ratings
            .SingleOrDefaultAsync(r => r.UserId == userId && r.MangaId == mangaId);

        return rating == null ? NotFound() : Ok(rating.Value);
    }

    [HttpPut]
    public async Task<IActionResult> PutMyRatingForManga(string mangaId, [FromForm] int inputRating)
    {
        if (inputRating < 1 || inputRating > 5)
        {
            return BadRequest("Invalid manga id or rating value.");
        }

        // rating exists
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentRating = await _context.Ratings
            .SingleOrDefaultAsync(r => r.UserId == userId && r.MangaId == mangaId);
        if (currentRating != null)
        {
            currentRating.Value = inputRating;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // rating not exists yet
        var manga = await _context.Mangas.FindAsync(mangaId);
        if (manga == null)
        {
            return NotFound("Manga not found");
        }

        var rating = new Rating()
        {
            Value = inputRating,
            UserId = userId,
            MangaId = mangaId
        };
        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMyRatingForManga), new { mangaId }, inputRating);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMyRatingForManga(string mangaId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var rating = await _context.Ratings
            .SingleOrDefaultAsync(r => r.UserId == userId && r.MangaId == mangaId);
        if (rating == null)
        {
            return NotFound();
        }

        _context.Ratings.Remove(rating);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
