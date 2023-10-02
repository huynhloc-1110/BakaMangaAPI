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

[Route("comments")]
[ApiController]
public partial class CommentController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public CommentController(ApplicationDbContext context, IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPut("{commentId}")]
    [Authorize]
    public async Task<IActionResult> PutComment(string commentId, [FromForm] CommentEditDTO commentDTO)
    {
        var comment = await _context.Comments
            .Include(c => c.User)
            .SingleOrDefaultAsync(c => c.Id == commentId);
        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        if (!await IsUserComment(User, comment))
        {
            return BadRequest("The comment is not owned by the current user");
        }

        comment = _mapper.Map(commentDTO, comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{commentId}")]
    [Authorize]
    public async Task<IActionResult> DeleteComment(string commentId)
    {
        var comment = await _context.Comments
            .Include(c => c.User)
            .SingleOrDefaultAsync(c => c.Id == commentId);
        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        if (!await IsUserComment(User, comment))
        {
            return BadRequest("The comment is not owned by the current user");
        }

        comment.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> IsUserComment(ClaimsPrincipal principal, Comment comment)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (comment.User == user)
        {
            return true;
        }
        return false;
    }
}
