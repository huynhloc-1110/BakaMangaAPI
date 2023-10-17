using System.Security.Claims;

using AutoMapper;

using BakaMangaAPI.Data;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.User;

[ApiController]
[Authorize]
public partial class RequestController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public RequestController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
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
        var request = await _context.Requests.FindAsync(requestId);
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
        return NoContent();
    }
}
