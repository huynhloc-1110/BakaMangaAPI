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
    public async Task<IActionResult> GetMyFollowings()
    {
        var user = await _userManager.GetUserAsync(User);
        var followings = await _context.ApplicationUsers
            .Where(u => u.Followers.Contains(user))
            .ProjectTo<UserSimpleDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return Ok(followings);
    }

    [HttpGet("~/followers")]
    public async Task<IActionResult> GetMyFollowers()
    {
        var user = await _userManager.GetUserAsync(User);
        var followers = await _context.ApplicationUsers
            .Where(u => u.Followings.Contains(user))
            .ProjectTo<UserSimpleDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return Ok(followers);
    }

    [HttpGet]
    public async Task<bool> GetMyFollowForUser(string userId)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _context.ApplicationUsers
            .AnyAsync(m => m.Id == userId &&
                m.Followers.Select(f => f.Id).Any(id => id == currentUserId));
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> PostMyFollowForManga(string userId)
    {
        var targetedUser = await _context.ApplicationUsers.FindAsync(userId);
        if (targetedUser == null)
        {
            return NotFound("User not found");
        }

        if (await GetMyFollowForUser(userId))
        {
            return BadRequest("User has already followed this manga.");
        }

        var currentUser = await _userManager.GetUserAsync(User);
        targetedUser.Followers.Add(currentUser);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMyFollowForUser), new { userId }, true);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveMyFollowForManga(string userId)
    {
        var targetedUser = await _context.ApplicationUsers
            .Include(m => m.Followers)
            .SingleOrDefaultAsync(m => m.Id == userId);
        if (targetedUser == null)
        {
            return NotFound("User not found");
        }

        var currentUser = await _userManager.GetUserAsync(User);
        targetedUser.Followers.Remove(currentUser);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}