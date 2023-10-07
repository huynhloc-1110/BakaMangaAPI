using System.Security.Claims;

using AutoMapper;
using AutoMapper.QueryableExtensions;

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
public partial class PostController : ControllerBase
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

    [HttpPut("{postId}")]
    [Authorize]
    public async Task<IActionResult> PutPost(string postId, [FromForm] PostEditDTO dto)
    {
        var post = await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Images)
            .SingleOrDefaultAsync(p => p.Id == postId);

        // validate
        if (post == null)
        {
            return NotFound();
        }
        if (!IsPostOwner(post, User))
        {
            return Forbid();
        }

        post.Content = dto.Content;
        await DeleteAllImagesFrom(post);
        await AddImagesTo(post, dto.Images);
        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<PostBasicDTO>(post));
    }

    [HttpDelete("{postId}")]
    [Authorize]
    public async Task<IActionResult> DeletePost(string postId)
    {
        var post = await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Images)
            .SingleOrDefaultAsync(p => p.Id == postId);

        // validate
        if (post == null)
        {
            return NotFound();
        }
        if (!IsPostOwner(post, User))
        {
            return Forbid();
        }

        await DeleteAllImagesFrom(post);
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool IsPostOwner(Post post, ClaimsPrincipal principal)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return post.User.Id == userId;
    }

    private async Task DeleteAllImagesFrom(Post post)
    {
        foreach (var image in post.Images)
        {
            await _mediaManager.DeleteImageAsync(image.Path);
        }
        _context.Images.RemoveRange(post.Images);
    }

    private async Task AddImagesTo(Post post, List<IFormFile> images)
    {
        post.Images = new();
        for (int i = 0; i < images.Count; i++)
        {
            var image = images[i];
            var imageId = Guid.NewGuid().ToString();
            var imagePath = await _mediaManager.UploadImageAsync(image, imageId, ImageType.Post);
            post.Images.Add(new() { Id = imageId, Number = i, Path = imagePath });
        }
    }
}
