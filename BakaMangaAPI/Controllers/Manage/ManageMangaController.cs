using AutoMapper;
using AutoMapper.QueryableExtensions;

using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services.Media;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Manage;

[Route("manage/mangas")]
[ApiController]
[Authorize(Roles = "Manager,Admin")]
public class ManageMangaController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediaManager _mediaManager;

    public ManageMangaController(ApplicationDbContext context, IMapper mapper, IMediaManager mediaManager)
    {
        _context = context;
        _mapper = mapper;
        _mediaManager = mediaManager;
    }

    [HttpGet("{mangaId}")]
    public async Task<IActionResult> GetManga(string mangaId)
    {
        var manga = await _context.Mangas
            .Where(m => m.Id == mangaId)
            .IgnoreQueryFilters()
            .ProjectTo<MangaDetailDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .SingleOrDefaultAsync();
        if (manga == null)
        {
            return NotFound();
        }

        return Ok(manga);
    }

    [HttpPost]
    public async Task<IActionResult> PostManga([FromForm] MangaEditDTO mangaEditDTO)
    {
        var manga = _mapper.Map<Manga>(mangaEditDTO);
        manga.Categories = await ConvertCategoriesAysnc(mangaEditDTO.CategoryIds);
        manga.Authors = await ConvertAuthorsAsync(mangaEditDTO.AuthorIds);

        // process image
        if (mangaEditDTO.CoverImage != null)
        {
            manga.CoverPath = await _mediaManager.UploadImageAsync(
                mangaEditDTO.CoverImage,
                manga.Id,
                ImageType.Cover);
        }

        _context.Mangas.Add(manga);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (MangaExists(manga.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        var mangaDetailDTO = _mapper.Map<MangaDetailDTO>(manga);
        return CreatedAtAction(nameof(GetManga), new { mangaId = mangaDetailDTO.Id }, mangaDetailDTO);
    }

    [HttpPut("{mangaId}")]
    public async Task<IActionResult> PutManga(string mangaId, [FromForm] MangaEditDTO mangaEditDTO)
    {
        var manga = await _context.Mangas
            .IgnoreQueryFilters()
            .Include(m => m.Authors)
            .Include(m => m.Categories)
            .SingleOrDefaultAsync(m => m.Id == mangaId);
        if (manga == null)
        {
            return NotFound();
        }

        manga = _mapper.Map(mangaEditDTO, manga);
        manga.Categories = await ConvertCategoriesAysnc(mangaEditDTO.CategoryIds);
        manga.Authors = await ConvertAuthorsAsync(mangaEditDTO.AuthorIds);

        // process image
        if (mangaEditDTO.CoverImage != null)
        {
            manga.CoverPath = await _mediaManager.UploadImageAsync(
                mangaEditDTO.CoverImage,
                manga.Id,
                ImageType.Cover);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MangaExists(mangaId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{mangaId}")]
    public async Task<IActionResult> DeleteManga(string mangaId, [FromQuery] bool undelete)
    {
        var manga = await _context.Mangas
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(m => m.Id == mangaId);
        if (manga == null)
        {
            return NotFound();
        }

        manga.DeletedAt = undelete ? null : DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool MangaExists(string id)
    {
        return _context.Mangas.IgnoreQueryFilters().Any(e => e.Id == id);
    }

    private async Task<List<Category>> ConvertCategoriesAysnc(string categoryIds)
    {
        var categoryArr = categoryIds.Split(',');
        return await _context.Categories
            .Where(c => categoryArr.Contains(c.Id))
            .ToListAsync();
    }

    private async Task<List<Author>> ConvertAuthorsAsync(string authorIds)
    {
        var authorArr = authorIds.Split(',');
        return await _context.Authors
            .Where(a => authorArr.Contains(a.Id))
            .ToListAsync();
    }
}
