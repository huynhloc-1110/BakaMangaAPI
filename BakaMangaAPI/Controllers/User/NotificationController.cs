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
[Route("my-notifications")]
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

    [HttpGet("counts")]
    public async Task<IActionResult> GetMyNotificationCounts()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var notificationCounts = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new
            {
                Request = u.Notifications.OfType<RequestNotification>().Count(),
                Chapter = u.Notifications.OfType<ChapterNotification>().Count(),
                Group = u.Notifications.OfType<GroupNotification>().Count(),
                Follower = u.Notifications.OfType<FollowerNotification>().Count(),
            })
            .SingleOrDefaultAsync();

        return Ok(notificationCounts);
    }


    [HttpGet("my-request-notifications")]
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
