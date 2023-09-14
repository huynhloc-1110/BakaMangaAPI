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
                Biography = g.Biography,
                CreatedAt = g.CreatedAt,
                MemberNumber = g.Members.Count(),
                UploadedChapterNumber = g.Chapters.Count(),
                ViewGainedNumber = g.Chapters.Sum(c => c.ChapterViews.Count)
            })
            .SingleOrDefaultAsync();

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
                m.IsLeader
            })
            .AsNoTracking()
            .ToListAsync();

        return Ok(members);
    }

    [HttpGet("{groupId}/chaptersByManga")]
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
                Manga = _mapper.Map<MangaBasicDTO>(g.Manga),
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
            Members = new() { new GroupMember { User = user, IsLeader = true } }
        };

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<GroupBasicDTO>(group));
    }
}
