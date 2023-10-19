using System.Security.Claims;

using AutoMapper;

using BakaMangaAPI.Data;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services.Notification;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.User;

[ApiController]
[Authorize]
public partial class RequestController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IHubContext<NotificationHub> _notificationHubContext;
    private readonly IUserConnectionManager _userConnectionManager;

    public RequestController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        IHubContext<NotificationHub> notificationHubContext,
        IUserConnectionManager userConnectionManager)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
        _notificationHubContext = notificationHubContext;
        _userConnectionManager = userConnectionManager;
    }

    [HttpPut("~/requests/{requestId}/status-confirm")]
    public async Task<ActionResult> ConfirmRequestStatus(string requestId)
    {
        var request = await _context.Requests
            .Include(r => r.User)
            .SingleOrDefaultAsync(r => r.Id == requestId);

        if (request == null)
        {
            return NotFound("Request not found");
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (request.User.Id != currentUserId)
        {
            return BadRequest("The user must be the owner of the request to confirm status");
        }

        request.StatusConfirmed = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("~/requests/{requestId}/status")]
    public async Task<ActionResult> ChangeRequestStatus(string requestId,
        [FromForm] RequestStatus status)
    {
        var request = await _context.Requests
            .Include(r => r.User)
            .SingleOrDefaultAsync(r => r.Id == requestId);
        if (request == null)
        {
            return NotFound("Request not found");
        }

        if (request.Status != RequestStatus.Processing)
        {
            return Forbid();
        }

        request.Status = status;

        await _context.SaveChangesAsync();
        await SendToUserAsync(request.User.Id, $"Request - {status}!");
        return NoContent();
    }

    private async Task SendToUserAsync(string userId, string message)
    {
        var connections = _userConnectionManager.GetUserConnections(userId);
        if (connections != null && connections.Count > 0)
        {
            foreach (var connectionId in connections)
            {
                await _notificationHubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
            }
        }
    }
}
