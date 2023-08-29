using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using BakaMangaAPI.Models;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Services;

namespace BakaMangaAPI.Controllers;

[Route("profile")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    private readonly IMediaManager _mediaManager;

    public UserController(UserManager<ApplicationUser> userManager, IMapper mapper, IMediaManager mediaManager)
    {
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

        return Ok(imagePath);
    }
}
