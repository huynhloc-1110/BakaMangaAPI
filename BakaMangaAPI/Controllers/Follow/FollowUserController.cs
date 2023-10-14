using System.Security.Claims;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Follow;

[Route("users/{userId}/my-follow")]
[ApiController]
[Authorize]
public class FollowUserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public FollowUserController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet("~/followings")]
    public async Task<IActionResult> GetMyFollowings([FromQuery] DateTime? followedAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var followings = await _context.ApplicationUserFollows
            .Where(f => f.UserId == currentUserId)
            .Where(f => followedAtCursor == null || f.FollowedAt < followedAtCursor)
            .OrderByDescending(f => f.FollowedAt)
            .Take(4)
            .ProjectTo<UserFollowDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        return Ok(followings);
    }

    [HttpGet("~/followers")]
    public async Task<IActionResult> GetMyFollowers([FromQuery] DateTime? followedAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var followers = await _context.ApplicationUserFollows
            .Where(f => f.FollowedUserId == currentUserId)
            .Where(f => followedAtCursor == null || f.FollowedAt < followedAtCursor)
            .OrderByDescending(f => f.FollowedAt)
            .Take(4)
            .ProjectTo<UserFollowDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        return Ok(followers);
    }

    [HttpGet]
    public async Task<bool> GetMyFollowForUser(string userId)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _context.ApplicationUserFollows
            .AnyAsync(f => f.UserId == currentUserId && f.FollowedUserId == userId);
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> PostMyFollowForUser(string userId)
    {
        var targetedUser = await _context.ApplicationUsers.FindAsync(userId);
        if (targetedUser == null)
        {
            return NotFound("User not found");
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var newFollow = new ApplicationUserFollow
        {
            UserId = currentUserId,
            FollowedUserId = userId
        };

        try
        {
            _context.ApplicationUserFollows.Add(newFollow);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMyFollowForUser), new { userId }, true);
        }
        catch (DbUpdateException)
        {
            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveMyFollowForManga(string userId)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var follow = await _context.ApplicationUserFollows
            .SingleOrDefaultAsync(f => f.UserId == currentUserId && f.FollowedUserId == userId);
        if (follow == null)
        {
            return NotFound("Follow not found");
        }

        _context.ApplicationUserFollows.Remove(follow);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
