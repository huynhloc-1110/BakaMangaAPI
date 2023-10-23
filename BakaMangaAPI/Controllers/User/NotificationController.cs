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
[Authorize]
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


    [HttpGet]
    public async Task<IActionResult> GetMyNotifications(
        [FromQuery] string type,
        [FromQuery] DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var notificationQuery = _context.Notifications
            .Where(n => n.User.Id == currentUserId)
            .Where(n => createdAtCursor == null || n.CreatedAt < createdAtCursor)
            .OrderByDescending(n => n.CreatedAt);

        switch (type)
        {
            case "Request":
                {
                    var notifications = await notificationQuery
                        .OfType<RequestNotification>()
                        .ProjectTo<RequestNotificationDTO>(_mapper.ConfigurationProvider)
                        .Take(4)
                        .AsNoTracking()
                        .ToListAsync();
                    return Ok(notifications);
                }
            case "Chapter":
                {
                    var notifications = await notificationQuery
                        .OfType<ChapterNotification>()
                        .ProjectTo<ChapterNotificationDTO>(_mapper.ConfigurationProvider)
                        .Take(4)
                        .AsNoTracking()
                        .ToListAsync();
                    return Ok(notifications);
                }
            case "Group":
                {
                    var notifications = await notificationQuery
                        .OfType<GroupNotification>()
                        .ProjectTo<GroupNotificationDTO>(_mapper.ConfigurationProvider)
                        .Take(4)
                        .AsNoTracking()
                        .ToListAsync();
                    return Ok(notifications);
                }
            case "Follower":
                {
                    var notifications = await notificationQuery
                        .OfType<FollowerNotification>()
                        .ProjectTo<FollowerNotificationDTO>(_mapper.ConfigurationProvider)
                        .Take(4)
                        .AsNoTracking()
                        .ToListAsync();
                    return Ok(notifications);
                }
            default:
                return BadRequest("Invalid notification type");
        }
    }

    [HttpPut("{notificationId}/view")]
    public async Task<IActionResult> ViewNotification(string notificationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var notification = await _context.Notifications
            .SingleOrDefaultAsync(n => n.Id == notificationId && n.User.Id == userId);

        if (notification == null)
        {
            return NotFound("Notification not found");
        }

        notification.IsViewed = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
