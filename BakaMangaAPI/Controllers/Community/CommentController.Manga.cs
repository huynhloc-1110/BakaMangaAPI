using System.Security.Claims;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Community;

public partial class CommentController
{
    [HttpGet("~/mangas/{mangaId}/comments")]
    public async Task<IActionResult> GetMangaComments(string mangaId, 
        [FromQuery] DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var comments = await _context.Comments
            .OfType<MangaComment>()
            .Where(c => c.Manga.Id == mangaId)
            .Where(c => createdAtCursor == null || c.CreatedAt < createdAtCursor)
            .OrderByDescending(p => p.CreatedAt)
            .Take(4)
            .Select(c => new CommentDTO
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                User = new UserSimpleDTO
                {
                    Id = c.User.Id,
                    Name = c.User.Name,
                    AvatarPath = c.User.AvatarPath
                },
                LikeCount = c.Reacts.Count(r => r.ReactFlag == ReactFlag.Like),
                DislikeCount = c.Reacts.Count(r => r.ReactFlag == ReactFlag.Dislike),
                ChildCommentCount = c.ChildComments.Count,
                UserReactFlag = c.Reacts
                    .Where(r => r.UserId == currentUserId)
                    .Select(r => r.ReactFlag)
                    .SingleOrDefault()
            })
            .ToListAsync();

        return Ok(comments);
    }

    [HttpPost("~/mangas/{mangaId}/comments")]
    [Authorize]
    public async Task<IActionResult> PostCommentForManga(string mangaId,
        [FromForm] CommentEditDTO commentDTO)
    {
        var manga = await _context.Mangas.FindAsync(mangaId);
        if (manga == null)
        {
            return NotFound("Manga not found");
        }

        var comment = _mapper.Map<MangaComment>(commentDTO);
        comment.User = await _userManager.GetUserAsync(User);
        comment.Manga = manga;

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<CommentDTO>(comment));
    }
}
