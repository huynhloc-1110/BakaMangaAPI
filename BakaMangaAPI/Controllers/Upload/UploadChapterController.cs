using AutoMapper;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BakaMangaAPI.Controllers;

[ApiController]
[Authorize(Roles = "Uploader")]
[Route("chapters")]
public class UploadChapterController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMediaManager _mediaManager;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public UploadChapterController(ApplicationDbContext context, IMediaManager mediaManager,
        IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _mediaManager = mediaManager;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> PostChapter([FromForm] ChapterEditDTO dto)
    {
        var manga = await _context.Mangas.FindAsync(dto.MangaId);
        if (manga == null)
        {
            return BadRequest("Manga not found");
        }
        var group = await _context.Groups.FindAsync(dto.UploadingGroupId);
        if (group == null)
        {
            return BadRequest("Group not found");
        }
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest("JWT token outdated or corrupted");
        }

        var chapter = new Chapter()
        {
            Number = dto.Number,
            Name = dto.Name,
            Language = dto.Language,
            Manga = manga,
            UploadingGroup = group,
            Uploader = user
        };

        for (int i = 0; i < dto.Pages.Count; i++)
        {
            var page = dto.Pages[i];
            var pageId = Guid.NewGuid().ToString();
            var pagePath = await _mediaManager.UploadImageAsync(page, pageId, ImageType.Page);
            chapter.Pages.Add(new() { Id = pageId, Number = i, Path = pagePath });
        }

        _context.Chapters.Add(chapter);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<ChapterDetailDTO>(chapter));
    }
}
