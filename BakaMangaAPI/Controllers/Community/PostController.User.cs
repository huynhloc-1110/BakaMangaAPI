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
    [HttpGet("~/users/{userId}/posts")]
    public async Task<IActionResult> GetUserPosts(string userId, [FromQuery] DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var posts = await _context.Posts
            .OfType<UserPost>()
            .Where(p => p.User.Id == userId)
            .Where(p => createdAtCursor == null || p.CreatedAt < createdAtCursor)
            .OrderByDescending(p => p.CreatedAt)
            .Take(4)
            .ProjectTo<PostBasicDTO>(_mapper.ConfigurationProvider, new { currentUserId })
            .ToListAsync();

        return Ok(posts);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostUserPost([FromForm] PostEditDTO dto)
    {
        var user = await _userManager.GetUserAsync(User);
        var post = _mapper.Map<UserPost>(dto);

        post.User = user;

        for (int i = 0; i < dto.Images.Count; i++)
        {
            var image = dto.Images[i];
            var imageId = Guid.NewGuid().ToString();
            var imagePath = await _mediaManager.UploadImageAsync(image, imageId, ImageType.Post);
            post.Images.Add(new() { Id = imageId, Number = i, Path = imagePath });
        }

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<PostBasicDTO>(post));
    }
}
