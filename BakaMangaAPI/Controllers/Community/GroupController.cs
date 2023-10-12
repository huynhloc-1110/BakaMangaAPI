using AutoMapper;
using AutoMapper.QueryableExtensions;

using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services.Media;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Community;

[ApiController]
[Route("groups")]
public partial class GroupController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediaManager _mediaManager;

    public GroupController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
        IMapper mapper, IMediaManager mediaManager)
    {
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
        _mediaManager = mediaManager;
    }

    [HttpGet("~/users/{userId}/groups")]
    public async Task<IActionResult> GetUserGroups(string userId, DateTime? joinedAtCursor)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var groups = await _context.Groups
            .Where(g => g.Members.Select(m => m.User).Contains(user))
            .Where(g => joinedAtCursor == null || g.Members.Single(m => m.User == user).JoinedAt < joinedAtCursor)
            .OrderByDescending(g => g.Members.Single(m => m.User == user).JoinedAt)
            .ProjectTo<GroupBasicDTO>(_mapper.ConfigurationProvider, new { userId })
            .Take(4)
            .AsNoTracking()
            .ToListAsync();

        return Ok(groups);
    }

    [HttpGet("{groupId}")]
    public async Task<IActionResult> GetGroup(string groupId)
    {
        var group = await _context.Groups
            .Where(g => g.Id == groupId)
            .ProjectTo<GroupDetailDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (group == null)
        {
            return NotFound();
        }

        return Ok(group);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostGroup([FromForm] GroupEditDTO dto)
    {
        var user = await _userManager.GetUserAsync(User);
        var group = _mapper.Map<Group>(dto);

        // the one who create the group is the initial owner
        group.Members = new() { new GroupMember { User = user, GroupRoles = GroupRole.Owner } };

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        var groupDetailDTO = _mapper.Map<GroupDetailDTO>(group);
        return CreatedAtAction(nameof(GetGroup), new { groupId = groupDetailDTO.Id }, groupDetailDTO);
    }

    [HttpPut("{groupId}/avatar")]
    [Authorize]
    public async Task<IActionResult> PutGroupAvatar(string groupId, IFormFile image)
    {
        if (await _context.Groups.FindAsync(groupId) is not Group group)
        {
            return NotFound("Group not found");
        }
        if (!await IsUserOfRoleOrHigher(group, GroupRole.Moderator))
        {
            return Forbid();
        }

        group.AvatarPath = await _mediaManager.UploadImageAsync(
            image, groupId, ImageType.Avatar);

        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<GroupDetailDTO>(group));
    }

    [HttpPut("{groupId}/banner")]
    [Authorize]
    public async Task<IActionResult> PutGroupBanner(string groupId, IFormFile image)
    {
        if (await _context.Groups.FindAsync(groupId) is not Group group)
        {
            return NotFound("Group not found");
        }
        if (!await IsUserOfRoleOrHigher(group, GroupRole.Moderator))
        {
            return Forbid();
        }

        group.BannerPath = await _mediaManager.UploadImageAsync(
            image, groupId, ImageType.Banner);

        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<GroupDetailDTO>(group));
    }

    [HttpPut("{groupId}")]
    [Authorize]
    public async Task<IActionResult> PutGroup(string groupId, [FromForm] GroupEditDTO dto)
    {
        if (await _context.Groups.FindAsync(groupId) is not Group group)
        {
            return NotFound("Group not found");
        }
        if (!await IsUserOfRoleOrHigher(group, GroupRole.Moderator))
        {
            return Forbid();
        }

        _mapper.Map(dto, group);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{groupId}")]
    [Authorize]
    public async Task<IActionResult> DeleteGroup(string groupId)
    {
        if (await _context.Groups.FindAsync(groupId) is not Group group)
        {
            return BadRequest("Group not found");
        }
        if (!await IsUserOfRoleOrHigher(group, GroupRole.Owner))
        {
            return Forbid();
        }

        group.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private async Task<bool> IsUserOfRoleOrHigher(Group group, GroupRole groupRole)
    {
        var user = await _userManager.GetUserAsync(User);
        return await _context.GroupMembers
            .Where(gm => gm.Group == group)
            .AnyAsync(gm => gm.User == user && gm.GroupRoles >= groupRole);
    }
}
