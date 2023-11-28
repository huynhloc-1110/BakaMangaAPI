using System.Security.Claims;

using AutoMapper.QueryableExtensions;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services.Media;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Community;

public partial class PostController
{
    [HttpGet("~/groups/{groupId}/posts")]
    public async Task<IActionResult> GetGroupPosts(string groupId,
        [FromQuery] DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var posts = await _context.Posts
            .OfType<GroupPost>()
            .Where(p => p.Group.Id == groupId)
            .Where(p => createdAtCursor == null || p.CreatedAt < createdAtCursor)
            .OrderByDescending(p => p.CreatedAt)
            .Take(4)
            .ProjectTo<PostBasicDTO>(_mapper.ConfigurationProvider, new { currentUserId })
            .ToListAsync();

        return Ok(posts);
    }

    [HttpPost("~/groups/{groupId}/posts")]
    [Authorize]
    public async Task<IActionResult> PostGroupPost(string groupId, [FromForm] PostEditDTO dto)
    {
        var group = await _context.Groups
            .Include(g => g.Members)
                .ThenInclude(m => m.User)
            .SingleOrDefaultAsync(g => g.Id == groupId);
        if (group == null)
        {
            return NotFound("Group not found");
        }

        // validate group member
        var user = await _userManager.GetUserAsync(User);
        var groupMember = await _context.GroupMembers.SingleOrDefaultAsync(
            m => m.User == user && m.Group == group);
        if (groupMember == null)
        {
            return Forbid();
        }

        var post = _mapper.Map<GroupPost>(dto);
        post.User = user;
        post.Group = group;

        for (int i = 0; i < dto.Images.Count; i++)
        {
            var image = dto.Images[i];
            var imageId = Guid.NewGuid().ToString();
            var imagePath = await _mediaManager.UploadImageAsync(image, imageId, ImageType.Post);
            post.Images.Add(new() { Id = imageId, Number = i, Path = imagePath });
        }

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        await _notificationManager.HandleGroupNotificationAsync(group);
        return Ok(_mapper.Map<PostBasicDTO>(post));
    }
}
