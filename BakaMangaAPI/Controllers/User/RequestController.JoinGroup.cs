using System.Security.Claims;
using AutoMapper.QueryableExtensions;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.User;

public partial class RequestController
{
    [HttpGet("~/groups/{groupId}/requests")]
    public async Task<IActionResult> GetGroupRequests(string groupId,
        [FromQuery] FilterDTO filter)
    {
        var group = await _context.Groups.FindAsync(groupId);
        if (group == null)
        {
            return NotFound("Group not found");
        }
        if (!await IsModOrHigherAsync(group))
        {
            return Forbid();
        }

        var requestQuery = _context.Requests
            .OfType<JoinGroupRequest>()
            .Where(r => r.Status == RequestStatus.Processing)
            .Where(r => r.Group.Id == groupId);

        if (!string.IsNullOrEmpty(filter.Search))
        {
            requestQuery = requestQuery.Where(r => r.User.Name.ToLower()
                .Contains(filter.Search.ToLower()));
        }

        var requestCount = await requestQuery.CountAsync();
        var requests = await requestQuery
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<JoinGroupRequestDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        var paginatedList = new PaginatedListDTO<JoinGroupRequestDTO>
            (requests, requestCount, filter.Page, filter.PageSize);
        return Ok(paginatedList);
    }

    [HttpGet("~/users/me/group-requests")]
    public async Task<IActionResult> GetMyGroupRequests([FromQuery] DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var requests = await _context.Requests
            .OfType<JoinGroupRequest>()
            .Where(r => r.User.Id == currentUserId)
            .Where(r => createdAtCursor == null || r.CreatedAt < createdAtCursor)
            .OrderByDescending(r => r.CreatedAt)
            .ProjectTo<JoinGroupRequestDTO>(_mapper.ConfigurationProvider)
            .Take(4)
            .ToListAsync();

        return Ok(requests);
    }

    [HttpPost("~/groups/{groupId}/requests")]
    public async Task<IActionResult> PostJoinGroupRequest(string groupId)
    {
        var group = await _context.Groups.FindAsync(groupId);
        if (group == null)
        {
            return NotFound("Group not found");
        }

        var currentUser = await _userManager.GetUserAsync(User);
        var isAlreadyMember = await _context.GroupMembers
            .AnyAsync(m => m.User == currentUser);
        if (isAlreadyMember)
        {
            return BadRequest("The current user is already member");
        }

        var isProcessing = await _context.Requests
            .OfType<JoinGroupRequest>()
            .AnyAsync(r => r.Group == group && r.User == currentUser && r.Status == RequestStatus.Processing);
        if (isProcessing)
        {
            return BadRequest("The user request is processed. Can't send any more requests");
        }

        var request = new JoinGroupRequest { Group = group, User = currentUser };
        _context.Requests.Add(request);

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("~/group-requests/{requestId}/status")]
    public async Task<ActionResult> ChangeJoinGroupRequestStatus(string requestId,
        [FromForm] RequestStatus status)
    {
        var request = _context.Requests
            .OfType<JoinGroupRequest>()
            .Include(r => r.User)
            .Include(r => r.Group)
            .SingleOrDefault(r => r.Id == requestId);
        if (request == null)
        {
            return NotFound("Request not found");
        }

        if (request.Status != RequestStatus.Processing || !await IsModOrHigherAsync(request.Group))
        {
            return Forbid();
        }

        request.Status = status;
        if (status == RequestStatus.Approve)
        {
            var groupMember = new GroupMember
            {
                User = request.User,
                Group = request.Group
            };
            _context.GroupMembers.Add(groupMember);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("~/group-requests/{requestId}/status-confirm")]
    public async Task<ActionResult> ConfirmJoinGroupRequestStatus(string requestId)
    {
        var request = await _context.Requests
            .OfType<JoinGroupRequest>()
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

    private async Task<bool> IsModOrHigherAsync(Group group)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentMember = await _context.GroupMembers
            .SingleOrDefaultAsync(m => m.UserId == currentUserId && m.Group == group);

        return currentMember != null && currentMember.GroupRoles >= GroupRole.Moderator;
    }
}
