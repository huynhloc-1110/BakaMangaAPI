using System.Security.Claims;

using AutoMapper.QueryableExtensions;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Community;

public partial class GroupController
{
    [HttpGet("{groupId}/members")]
    public async Task<IActionResult> GetGroupMembers(string groupId,
        [FromQuery] GroupRole? roleUpperBound,
        [FromQuery] GroupRole? roleLowerBound,
        [FromQuery] DateTime? joinedAtCursor)
    {
        if (await _context.Groups.FindAsync(groupId) is not Group group)
        {
            return NotFound();
        }

        var members = await _context.GroupMembers
            .Where(m => m.Group == group)
            .Where(m => roleUpperBound == null || m.GroupRoles < roleUpperBound)
            .Where(m => roleLowerBound == null || m.GroupRoles >= roleLowerBound)
            .Where(m => joinedAtCursor == null || m.JoinedAt < joinedAtCursor)
            .OrderByDescending(m => m.JoinedAt)
            .Take(4)
            .ProjectTo<GroupMemberDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        return Ok(members);
    }

    [HttpGet("{groupId}/paginated-members")]
    [Authorize]
    public async Task<IActionResult> GetPaginatedGroupMembers(string groupId,
        [FromQuery] MemberFilterDTO filter)
    {
        var currentMember = await _context.GroupMembers
            .SingleOrDefaultAsync(m => 
                m.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier) &&
                m.GroupId == groupId);
        if (currentMember == null || currentMember.GroupRoles < GroupRole.Moderator)
        {
            return Forbid();
        }

        var memberQuery = _context.GroupMembers
            .Where(m => m.GroupId == groupId)
            .Where(m => filter.Search == null || m.User.Name.ToLower().Contains(filter.Search.ToLower()))
            .Where(m => m.GroupRoles.HasFlag(filter.GroupRoleOptions));

        var members = await memberQuery
            .OrderByDescending(m => m.JoinedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<GroupMemberDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        var memberCount = await memberQuery.CountAsync();

        var memberList = new PaginatedListDTO<GroupMemberDTO>(
            members, memberCount, filter.Page, filter.PageSize);
        return Ok(memberList);
    }


    [HttpGet("{groupId}/members/{memberId}")]
    public async Task<IActionResult> GetGroupMember(string groupId, string memberId)
    {
        var member = await _context.GroupMembers
            .Where(m => m.GroupId == groupId && m.UserId == memberId)
            .ProjectTo<GroupMemberDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        return member != null ? Ok(member) : NotFound();
    }

    [HttpPut("{groupId}/members/{memberId}/group-roles")]
    [Authorize]
    public async Task<IActionResult> PutGroupRolesOfMember(string groupId, string memberId,
        [FromForm] GroupRole groupRoles)
    {
        var targetedMember = await _context.GroupMembers
            .SingleOrDefaultAsync(gm => gm.GroupId == groupId && gm.User.Id == memberId);
        var currentMemberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentMember = await _context.GroupMembers
            .SingleOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == currentMemberId);

        if (targetedMember == null || currentMember == null)
        {
            return NotFound();
        }

        // role permission condition to continue:
        // - current user has owner role
        // - current user is a mod and the targetted is lower than mod
        var rolePermission =
            currentMember.GroupRoles.HasFlag(GroupRole.Owner) ||
            (currentMember.GroupRoles.HasFlag(GroupRole.Moderator)
                && targetedMember.GroupRoles < GroupRole.Moderator);
        if (!rolePermission)
        {
            return Forbid();
        }

        // prevent owner to drop ownership without transfer
        if (currentMember.GroupRoles.HasFlag(GroupRole.Owner)
            && targetedMember == currentMember
            && !groupRoles.HasFlag(GroupRole.Owner))
        {
            return BadRequest("The owner cannot drop group ownership " +
                "without transfering it to another member");
        }

        // condition to transfer ownership: the current user must has owner role
        if (groupRoles.HasFlag(GroupRole.Owner))
        {
            currentMember.GroupRoles -= GroupRole.Owner;
        }

        targetedMember.GroupRoles = groupRoles;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{groupId}/members")]
    [Authorize]
    public async Task<IActionResult> JoinGroup(string groupId)
    {
        var group = await _context.Groups.FindAsync(groupId);
        var user = await _userManager.GetUserAsync(User);
        if (group == null || user == null)
        {
            return NotFound();
        }

        var groupMember = new GroupMember
        {
            Group = group,
            User = user,
            GroupRoles = GroupRole.Member
        };

        _context.GroupMembers.Add(groupMember);
        await _context.SaveChangesAsync();

        var memberDTO = _mapper.Map<GroupMemberDTO>(groupMember);
        return Ok(memberDTO);
    }

    [HttpDelete("{groupId}/members/{memberId}")]
    [Authorize]
    public async Task<IActionResult> DeleteGroupMember(string groupId, string memberId)
    {
        var targetedMember = await _context.GroupMembers
            .SingleOrDefaultAsync(gm => gm.GroupId == groupId && gm.User.Id == memberId);
        var currentMemberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentMember = await _context.GroupMembers
            .SingleOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == currentMemberId);

        if (targetedMember == null || currentMember == null)
        {
            return NotFound();
        }

        // role permission condition
        // - current member is not owner and he remove himself
        // - current member is owner and he does not remove himself
        // - current member role is mod and the targetted is lower than mod
        var rolePermission =
            (!currentMember.GroupRoles.HasFlag(GroupRole.Owner)
                && currentMember == targetedMember) ||
            (currentMember.GroupRoles.HasFlag(GroupRole.Owner)
                && currentMember != targetedMember) ||
            (currentMember.GroupRoles.HasFlag(GroupRole.Moderator)
                && targetedMember.GroupRoles < GroupRole.Moderator);
        if (!rolePermission)
        {
            return Forbid();
        }

        _context.GroupMembers.Remove(targetedMember);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
