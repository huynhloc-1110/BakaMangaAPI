using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using BakaMangaAPI.Models;
using BakaMangaAPI.DTOs;

namespace BakaMangaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<UserBasicDTO> GetCurrentUserBasicInfo()
    {
		var currentUser = await _userManager.GetUserAsync(User);
		var userRoles = await _userManager.GetRolesAsync(currentUser);

		var userBasicDto = _mapper.Map<UserBasicDTO>(currentUser);
		userBasicDto.Roles = userRoles.ToList();

		return userBasicDto;
	}
}
