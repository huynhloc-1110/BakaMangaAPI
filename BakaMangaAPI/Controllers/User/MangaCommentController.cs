using AutoMapper;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("user/[controller]")]
[ApiController]
[Authorize]
public class MangaCommentController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public MangaCommentController(ApplicationDbContext context, IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> LoadMangaChildComments(string id,
        [FromQuery] FilterDTO filter)
    {
        var commentCount = await _context.MangaComments
            .Where(c => c.ParentComment!.Id == id)
            .CountAsync();
        var comments = await _context.MangaComments
            .Where(c => c.ParentComment!.Id == id)
            .Include(c => c.User)
            .Include(c => c.ChildComments)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var commentList = _mapper.Map<List<CommentDTO>>(comments);
        var paginatedCommentList = new PaginatedListDTO<CommentDTO>(
            commentList, commentCount, filter.Page, filter.PageSize);
        return Ok(paginatedCommentList);
    }
}
