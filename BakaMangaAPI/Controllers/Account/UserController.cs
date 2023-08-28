using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using BakaMangaAPI.Models;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Data;

namespace BakaMangaAPI.Controllers;

[Route("account/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public UserController(UserManager<ApplicationUser> userManager,
        ApplicationDbContext context, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
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
        var user = await _context.ApplicationUsers.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var userBasicDto = _mapper.Map<UserBasicDTO>(user);
        userBasicDto.Roles = userRoles.ToList();

        return Ok(userBasicDto);
    }
}
