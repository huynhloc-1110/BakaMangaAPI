using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using BakaMangaAPI.Models;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Services;
using BakaMangaAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers;

[Route("profile")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IMediaManager _mediaManager;

    public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
        IMapper mapper, IMediaManager mediaManager)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
        _mediaManager = mediaManager;
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserBasicInfo()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var userRoles = await _userManager.GetRolesAsync(currentUser);

        var userBasicDto = _mapper.Map<UserBasicDTO>(currentUser);
        userBasicDto.Roles = userRoles.ToList();

        return Ok(userBasicDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserBasicInfo(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var userBasicDto = _mapper.Map<UserBasicDTO>(user);
        userBasicDto.Roles = userRoles.ToList();

        return Ok(userBasicDto);
    }

    [HttpGet("{userId}/stats")]
    public async Task<IActionResult> GetUserStats(string userId)
    {
        // get user followers, following number, followed manga number,...
        // number of uploaded chapters, number of views
        var userStats = await _context.ApplicationUsers
            .Where(u => u.Id == userId)
            .Select(u => new UserStatsDTO
            {
                FollowerNumber = u.Followers.Where(f => f.DeletedAt == null).Count(),
                FollowingNumber = u.Followings.Where(f => f.DeletedAt == null).Count(),
                FollowedMangaNumber = u.FollowedMangas.Where(m => m.DeletedAt == null).Count(),
                UploadedChapterNumber = u.UploadedChapters.Where(c => c.DeletedAt == null).Count(),
                ViewGainedNumber = u.UploadedChapters.Sum(c => c.ChapterViews.Count)
            })
            .SingleOrDefaultAsync();

        return (userStats != null) ? Ok(userStats) : NotFound();        
    }

    [HttpPut("me/change-avatar")]
    [Authorize]
    public async Task<IActionResult> ChangeAvatar(IFormFile image)
    {
        if (image == null)
        {
            return BadRequest("Avatar image is null");
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest("Token corrupted");
        }

        var imagePath = await _mediaManager.UploadImageAsync(image, user.Id, ImageType.Avatar);
        user.AvatarPath = imagePath;
        await _userManager.UpdateAsync(user);

        return Ok(_mapper.Map<UserBasicDTO>(user));
    }

    [HttpPut("me/change-banner")]
    [Authorize]
    public async Task<IActionResult> ChangeBanner(IFormFile image)
    {
        if (image == null)
        {
            return BadRequest("Banner image is null");
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest("Token corrupted");
        }

        var imagePath = await _mediaManager.UploadImageAsync(image, user.Id, ImageType.Banner);
        user.BannerPath = imagePath;
        await _userManager.UpdateAsync(user);

        return Ok(_mapper.Map<UserBasicDTO>(user));
    }
}
