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

namespace BakaMangaAPI.Controllers.User;

[ApiController]
public class NotificationController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public NotificationController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet("my-request-notifications")]
    [Authorize]
    public async Task<IActionResult> GetMyRequestNotifications()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var notifications = await _context.Notifications
            .OfType<RequestNotification>()
            .Where(n => n.User.Id == currentUserId)
            .OrderByDescending(n => n.CreatedAt)
            .ProjectTo<RequestNotificationDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        return Ok(notifications);
    }

    [HttpGet("my-chapter-notifications")]
    [Authorize]
    public async Task<IActionResult> GetMyChapterNotifications()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var notifications = await _context.Notifications
            .OfType<ChapterNotification>()
            .Where(n => n.User.Id == currentUserId)
            .OrderByDescending(n => n.CreatedAt)
            .ProjectTo<ChapterNotificationDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        return Ok(notifications);
    }
}
