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

namespace BakaMangaAPI.Controllers.Upload;

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

    [HttpGet("~/users/{userId}/manga-groups")]
    [Authorize]
    public async Task<IActionResult> GetUserMangaGroups(string userId)
    {
        if (await _userManager.GetUserAsync(User) is not ApplicationUser user)
        {
            return Forbid();
        }

        var groups = await _context.Groups
            .Where(g => g.IsMangaGroup)
            .Where(g => g.Members.Single(m => m.User == user).GroupRoles.HasFlag(GroupRole.GroupUploader))
            .Select(g => new { g.Id, g.Name })
            .AsNoTracking()
            .ToListAsync();

        return Ok(groups);
    }

    [HttpGet("{groupId}/chapters-by-manga")]
    public async Task<IActionResult> GetChaptersOfUploaderByManga(string groupId,
        [FromQuery] FilterDTO filter)
    {
        if (await _context.Groups.FindAsync(groupId) is not Group group)
        {
            return NotFound();
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var mangaQuery = _context.Mangas
            .Where(m => m.Chapters.Select(c => c.UploadingGroup!.Id).Contains(groupId));

        var mangaCount = await mangaQuery.CountAsync();

        var mangas = await _context.Mangas
            .ProjectTo<GroupMangaBlockDTO>(_mapper.ConfigurationProvider, new { groupId, currentUserId })
            .OrderByDescending(g => g.UpdatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        var paginatedList = new PaginatedListDTO<GroupMangaBlockDTO>
            (mangas, mangaCount, filter.Page, filter.PageSize);

        return Ok(paginatedList);
    }
}
