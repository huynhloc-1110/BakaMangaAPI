using System.Security.Claims;

using AutoMapper;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services.Media;

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
    private readonly IMediaManager _mediaManager;

    public UploadGroupController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
        IMapper mapper, IMediaManager mediaManager)
    {
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
        _mediaManager = mediaManager;
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
                IsMangaGroup = g.IsMangaGroup,
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
                Biography = g.Biography,
                IsMangaGroup = g.IsMangaGroup,
                CreatedAt = g.CreatedAt,
                MemberNumber = g.Members.Count(),
                UploadedChapterNumber = g.Chapters.Count(),
                ViewGainedNumber = g.Chapters.Sum(c => c.ChapterViews.Count)
            })
            .SingleOrDefaultAsync();

        if (group == null)
        {
            return NotFound();
        }

        return Ok(group);
    }

    [HttpGet("{groupId}/members")]
    public async Task<IActionResult> GetGroupMembers(string groupId)
    {
        if (await _context.Groups.FindAsync(groupId) is not Group group)
        {
            return NotFound();
        }

        var members = await _context.GroupMembers
            .Where(m => m.Group == group)
            .Select(m => new
            {
                m.User.Id,
                m.User.Name,
                m.User.AvatarPath,
                m.GroupRoles
            })
            .AsNoTracking()
            .ToListAsync();

        return Ok(members);
    }

    [HttpGet("{groupId}/chapters-by-manga")]
    [AllowAnonymous]
    public async Task<IActionResult> GetChaptersOfUploaderByManga(string groupId,
        [FromQuery] FilterDTO filter)
    {
        if (await _context.Groups.FindAsync(groupId) is not Group group)
        {
            return NotFound();
        }

        var chapterGroupingCount = await _context.Mangas
            .Where(m => m.Chapters.Select(c => c.UploadingGroup).Contains(group))
            .CountAsync();

        var chapterGroupings = await _context.Chapters
            .Include(c => c.Uploader)
            .Include(c => c.UploadingGroup)
            .Include(c => c.ChapterViews)
            .Where(c => c.DeletedAt == null)
            .Where(c => c.UploadingGroup == group)
            .GroupBy(c => c.Manga.Id)
            .Select(g => new
            {
                Manga = _context.Mangas.SingleOrDefault(m => m.Id == g.Key),
                Chapters = g.OrderByDescending(c => c.CreatedAt).Take(3).ToList(),
                UpdatedAt = g.Max(c => c.CreatedAt)
            })
            .OrderByDescending(g => g.UpdatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        var chapterGroupingList = chapterGroupings
            .Select(g => new ChapterGroupingDTO()
            {
                Manga = _mapper.Map<MangaSimpleDTO>(g.Manga),
                Chapters = _mapper.Map<List<ChapterBasicDTO>>(g.Chapters)
            })
            .ToList();
        var paginatedList = new PaginatedListDTO<ChapterGroupingDTO>
            (chapterGroupingList, chapterGroupingCount, filter.Page, filter.PageSize);

        return Ok(paginatedList);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostGroup([FromForm] GroupEditDTO dto)
    {
        if (await _userManager.GetUserAsync(User) is not ApplicationUser user)
        {
            return BadRequest("Token outdated or corrupted");
        }
        var group = new Group
        {
            Name = dto.Name,
            Biography = dto.Biography,
            IsMangaGroup = dto.IsMangaGroup,
            Members = new() { new GroupMember { User = user, GroupRoles = GroupRole.Owner } }
        };

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<GroupBasicDTO>(group));
    }

    [HttpPut("{groupId}/avatar")]
    [Authorize]
    public async Task<IActionResult> PutGroupAvatar(string groupId, IFormFile image)
    {
        if (await _context.Groups.FindAsync(groupId) is not Group group)
        {
            return NotFound("Group not found");
        }
        if (!await IsUserOfRole(group, GroupRole.Moderator | GroupRole.Owner))
        {
            return Unauthorized("The user must be group moderator or owner to do this");
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
        if (!await IsUserOfRole(group, GroupRole.Moderator | GroupRole.Owner))
        {
            return Unauthorized("The user must be group moderator or owner to do this");
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
        if (!await IsUserOfRole(group, GroupRole.Moderator | GroupRole.Owner))
        {
            return Unauthorized("The user must be group moderator or owner to do this");
        }

        group.Name = dto.Name;
        group.Biography = dto.Biography;
        group.IsMangaGroup = dto.IsMangaGroup;
        
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
        if (!await IsUserOfRole(group, GroupRole.Owner))
        {
            return Unauthorized("The user must be group owner to do this");
        }

        group.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{groupId}/members/{memberId}/group-roles")]
    [Authorize]
    public async Task<IActionResult> PutGroupRolesOfMember(string groupId, string memberId,
        [FromForm] GroupRole groupRoles)
    {
        var targetedMember = await _context.GroupMembers
            .SingleOrDefaultAsync(gm => gm.Group.Id == groupId && gm.User.Id == memberId);
        var currentMember = await _context.GroupMembers
            .SingleOrDefaultAsync(m => m.UserId == User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (targetedMember == null || currentMember == null)
        {
            return NotFound();
        }

        // check if targeted member has smaller role
        if (targetedMember.GroupRoles >= currentMember.GroupRoles)
        {
            return Unauthorized("Your group role must be higher to change this member role");
        }

        // transfer ownership
        if (groupRoles.HasFlag(GroupRole.Owner))
        {
            currentMember.GroupRoles -= GroupRole.Owner;
        }

        targetedMember.GroupRoles = groupRoles;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> IsUserOfRole(Group group, GroupRole groupRole)
    {
        var user = await _userManager.GetUserAsync(User);
        return await _context.GroupMembers
            .Where(gm => gm.Group == group)
            .AnyAsync(gm => gm.User == user && gm.GroupRoles.HasFlag(groupRole));
    }
}
