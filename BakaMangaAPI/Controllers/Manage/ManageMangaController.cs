using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services;
using AutoMapper;

namespace BakaMangaAPI.Controllers;

[Route("manage/manga")]
[ApiController]
[Authorize(Roles = "Admin")]
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

    // GET: manage/manga/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetManga(string id)
    {
        var manga = await _context.Mangas
            .Include(m => m.Categories)
            .Include(m => m.Authors)
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);

        if (manga == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<MangaDetailDTO>(manga));
    }

    // PUT: manage/manga/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutManga([FromRoute] string id,
        [FromForm] MangaEditDTO mangaEditDTO, [FromForm] IFormFile? coverImage)
    {
        if (id != mangaEditDTO.Id)
        {
            return BadRequest();
        }

        var manga = await _context.Mangas
            .Include(m => m.Authors)
            .Include(m => m.Categories)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (manga == null)
        {
            return NotFound();
        }

        manga = _mapper.Map(mangaEditDTO, manga);

        // process categories
        manga.Categories = new();
        var categoryIds = mangaEditDTO.CategoryIds.Split(',');
        foreach (var categoryId in categoryIds)
        {
            var category = await _context.Categories
                .FindAsync(categoryId);
            if (category != null)
            {
                manga.Categories.Add(category);
            }
        }

        // process authors
        var authorIds = mangaEditDTO.AuthorIds.Split(',');
        foreach (var authorId in authorIds)
        {
            var author = await _context.Authors
                .FindAsync(authorId);
            if (author != null)
            {
                manga.Authors.Add(author);
            }
        }

        // process image
        if (coverImage != null)
        {
            manga.CoverPath = await _mediaManager.UploadImage(coverImage, manga.Id, ImageType.Cover);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MangaExists(id))
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

    // POST: manage/manga
    [HttpPost]
    public async Task<IActionResult> PostManga
        ([FromForm] MangaEditDTO mangaEditDTO, [FromForm] IFormFile? coverImage)
    {
        var manga = _mapper.Map<Manga>(mangaEditDTO);

        // process categories
        var categoryIds = mangaEditDTO.CategoryIds.Split(',');
        foreach (var categoryId in categoryIds)
        {
            var category = await _context.Categories
                .FindAsync(categoryId);
            if (category != null)
            {
                manga.Categories.Add(category);
            }
        }

        // process authors
        var authorIds = mangaEditDTO.AuthorIds.Split(',');
        foreach (var authorId in authorIds)
        {
            var author = await _context.Authors
                .FindAsync(authorId);
            if (author != null)
            {
                manga.Authors.Add(author);
            }
        }

        // process image
        if (coverImage != null)
        {
            manga.CoverPath = await _mediaManager.UploadImage(coverImage, manga.Id, ImageType.Cover);
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
        return CreatedAtAction("GetManga", new { id = mangaDetailDTO.Id }, mangaDetailDTO);
    }

    // DELETE: manage/manga/5?undelete=false
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteManga(string id, [FromQuery] bool undelete)
    {
        var manga = await _context.Mangas.FindAsync(id);
        if (manga == null)
        {
            return NotFound();
        }

        if (undelete)
        {
            manga.DeletedAt = null;
        }
        else
        {
            manga.DeletedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool MangaExists(string id)
    {
        return (_context.Mangas?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
