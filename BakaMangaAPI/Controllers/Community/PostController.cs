﻿using System.Security.Claims;

using AutoMapper;

using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services.Media;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Community;

[Route("posts")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediaManager _mediaManager;

    public PostController(ApplicationDbContext context,
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        IMediaManager mediaManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
        _mediaManager = mediaManager;
    }

    [HttpGet("~/users/{userId}/posts")]
    public async Task<IActionResult> GetUserPosts(string userId, [FromQuery] DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var posts = await _context.Posts
            .Where(p => p.User.Id == userId)
            .Where(p => createdAtCursor == null || p.CreatedAt < createdAtCursor)
            .OrderByDescending(p => p.CreatedAt)
            .Take(4)
            .Select(p => new PostBasicDTO
            {
                Id = p.Id,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                User = new UserSimpleDTO
                {
                    Id = p.User.Id,
                    Name = p.User.Name,
                    AvatarPath = p.User.AvatarPath
                },
                ImageUrls = p.Images.Select(i => i.Path).ToList(),
                LikeCount = p.Reacts.Count(r => r.ReactFlag == ReactFlag.Like),
                DislikeCount = p.Reacts.Count(r => r.ReactFlag == ReactFlag.Dislike),
                CommentCount = p.Comments.Count,
                UserReactFlag = p.Reacts
                    .Where(r => r.UserId == currentUserId)
                    .Select(r => r.ReactFlag)
                    .SingleOrDefault()
            })
            .ToListAsync();

        return Ok(posts);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostPost([FromForm] PostEditDTO dto)
    {
        var user = await _userManager.GetUserAsync(User);
        var post = _mapper.Map<Post>(dto);

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

    [HttpPut("{postId}")]
    [Authorize]
    public async Task<IActionResult> PutPost(string postId, [FromForm] PostEditDTO dto)
    {
        // validate
        var post = await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Images)
            .SingleOrDefaultAsync(p => p.Id == postId);
        if (post == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (post.User.Id != userId)
        {
            return Forbid();
        }

        // change properties
        post.Content = dto.Content;

        foreach (var image in post.Images)
        {
            await _mediaManager.DeleteImageAsync(image.Path);
        }
        _context.Images.RemoveRange(post.Images);

        post.Images = new();
        for (int i = 0; i < dto.Images.Count; i++)
        {
            var image = dto.Images[i];
            var imageId = Guid.NewGuid().ToString();
            var imagePath = await _mediaManager.UploadImageAsync(image, imageId, ImageType.Post);
            post.Images.Add(new() { Id = imageId, Number = i, Path = imagePath });
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{postId}")]
    [Authorize]
    public async Task<IActionResult> DeletePost(string postId)
    {
        // validate
        var post = await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Images)
            .SingleOrDefaultAsync(p => p.Id == postId);
        if (post == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (post.User.Id != userId)
        {
            return Forbid();
        }

        foreach (var image in post.Images)
        {
            await _mediaManager.DeleteImageAsync(image.Path);
        }

        _context.Images.RemoveRange(post.Images);
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
