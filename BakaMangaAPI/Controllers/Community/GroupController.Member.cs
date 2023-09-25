using System.Security.Claims;

using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Community;

public partial class GroupController
{
    [HttpGet("{groupId}/members")]
    public async Task<IActionResult> GetGroupMembers(string groupId)
    {
        if (await _context.Groups.FindAsync(groupId) is not Group group)
        {
            return NotFound();
        }

        var members = await _context.GroupMembers
            .Where(m => m.Group == group)
            .Select(m => new { m.User.Id, m.User.Name, m.User.AvatarPath, m.GroupRoles })
            .AsNoTracking()
            .ToListAsync();

        return Ok(members);
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
        // - current member role is owner and he does not remove himself
        // - curent member role is mod and the targetted is lower than mod
        var rolePermission =
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
