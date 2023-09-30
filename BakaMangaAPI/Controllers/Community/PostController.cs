using System.Security.Claims;

using AutoMapper;

using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

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

    public PostController(ApplicationDbContext context,
        IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
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
                Id = p.Id, Content = p.Content, CreatedAt = p.CreatedAt,
                User = new UserSimpleDTO { Id = p.User.Id, Name = p.User.Name },
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
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<PostBasicDTO>(post));
    }
}
