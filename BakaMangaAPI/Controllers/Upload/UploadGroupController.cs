using AutoMapper;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers;

[ApiController]
[Route("groups")]
public class UploadGroupController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UploadGroupController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
        IMapper mapper)
    {
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("~/users/{userId}/groups")]
    public async Task<IActionResult> GetUserGroups(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return BadRequest("JWT token outdated or corrupted");
        }

        var groups = await _context.Groups
            .Where(g => g.Members.Select(m => m.User).Contains(user))
            .Select(g => new GroupBasicDTO
            {
                Id = g.Id,
                Name = g.Name,
                MemberNumber = g.Members.Count(),
                AvatarPath = g.AvatarPath,
            })
            .ToListAsync();

        return Ok(groups);
    }

    [HttpGet("{groupId}")]
    public async Task<IActionResult> GetGroup(string groupId)
    {
        var group = await _context.Groups
            .Where(g => g.Id == groupId)
            .Select(g => new GroupDetailDTO
            {
                Id = g.Id,
                Name = g.Name,
                AvatarPath = g.AvatarPath,
                BannerPath = g.BannerPath,
                MemberNumber = g.Members.Count(),
                UploadedChapterNumber = g.Chapters.Count(),
                ViewGainedNumber = g.Chapters.Sum(c => c.ChapterViews.Count)
            })
            .SingleOrDefaultAsync();

        return Ok(group);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostGroup(GroupEditDTO dto)
    {
        if (await _userManager.GetUserAsync(User) is not ApplicationUser user)
        {
            return BadRequest("Token outdated or corrupted");
        }
        var group = new Group
        {
            Name = dto.Name,
            Biography = dto.Biography,
            Members = new() { new GroupMember { User = user, IsLeader = true } }
        };

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<GroupBasicDTO>(group));
    }
}
