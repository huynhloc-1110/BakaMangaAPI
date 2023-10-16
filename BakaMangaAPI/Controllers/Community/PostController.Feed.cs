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
public partial class PostController : ControllerBase
{
    [HttpGet("/my-feed")]
    [Authorize]
    public async Task<IActionResult> GetMyPostFeed(DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var upperBound = createdAtCursor == null ? DateTime.UtcNow : createdAtCursor.Value;
        var lowerBound = upperBound.AddDays(-7);

        var posts = await _context.Posts
            .Where(p => p.CreatedAt < upperBound && p.CreatedAt > lowerBound)
            .Select(p => new
            {
                Post = p,
                WeeklyViews = p.Views.Count(v => (DateTime.UtcNow - v.CreatedAt).Days <= 7),
                MaxViews = _context.Posts.Max(p => p.Views
                    .Count(cv => (DateTime.UtcNow - cv.CreatedAt).Days <= 7)),
                WeeklyReacts = p.Reacts.Count(v => (DateTime.UtcNow - v.CreatedAt).Days <= 7),
                MaxReacts = _context.Posts.Max(p => p.Reacts
                    .Count(cv => (DateTime.UtcNow - cv.CreatedAt).Days <= 7)),
                WeeklyComments = p.Comments.Count(v => (DateTime.UtcNow - v.CreatedAt).Days <= 7),
                MaxComments = _context.Posts.Max(p => p.Comments
                    .Count(cv => (DateTime.UtcNow - cv.CreatedAt).Days <= 7)),
                ExistingWeeks = (DateTime.UtcNow - p.CreatedAt).Days / 7
            })
            .Select(p => new
            {
                p.Post,
                ViewScore = (p.MaxViews > 0) ? (double)p.WeeklyViews * 10 / p.MaxViews : 0,
                ReactScore = (p.MaxReacts > 0) ? (double)p.WeeklyReacts * 10 / p.MaxReacts : 0,
                CommentScore = (p.MaxComments > 0) ? (double)p.WeeklyComments * 10 / p.MaxComments : 0,
                NewScore = 10 - (p.ExistingWeeks < 10 ? p.ExistingWeeks : 10),
                RelevantScore =
                    p.Post.User.Id == currentUserId ||
                    ((UserPost)p.Post).User.Followers.Select(f => f.UserId).Contains(currentUserId) ? 2 :
                    ((GroupPost)p.Post).Group.Members.Select(m => m.UserId).Contains(currentUserId) ? 1 : 0
            })
            .OrderByDescending(p => p.ViewScore * 2 + p.ReactScore * 2 + p.CommentScore + p.NewScore * 5 +
                p.RelevantScore * 50)
            .Select(p => p.Post)
            .ProjectTo<PostBasicDTO>(_mapper.ConfigurationProvider, new { currentUserId })
            .Take(12)
            .AsNoTracking()
            .ToListAsync();

        return Ok(posts);
    }
}
