using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services;
using AutoMapper;

namespace BakaMangaAPI.Controllers;

[Route("admin/manga")]
[ApiController]
//[Authorize(Roles = "Admin")]
public class AdminMangaController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediaManager _mediaManager;

    public AdminMangaController(ApplicationDbContext context, IMapper mapper, IMediaManager mediaManager)
    {
        _context = context;
        _mapper = mapper;
        _mediaManager = mediaManager;
    }

    // GET: api/Manga?Search=&Page=1&PageSize=12
    [HttpGet]
    public async Task<IActionResult> GetMangas
        ([FromQuery] FilterDTO filter)
    {
        var query = _context.Mangas.AsQueryable();
        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(m => m.OriginalTitle.ToLower().Contains(filter.Search.ToLower()) ||
                m.AlternativeTitles!.Contains(filter.Search));
        }
        var mangas = await query
            .OrderBy(m => m.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsNoTracking()
            .ToListAsync();
        return Ok(_mapper.Map<List<MangaBasicDTO>>(mangas));
    }

    // GET: api/Manga/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetManga(string id)
    {
        var manga = await _context.Mangas
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);

        if (manga == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<MangaDetailDTO>(manga));
    }

    // PUT: api/Manga/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutManga(string id, MangaDetailDTO mangaDTO)
    {
        if (id != mangaDTO.Id)
        {
            return BadRequest();
        }

        var manga = await _context.Mangas
            .FirstOrDefaultAsync(m => m.Id == id);
        if (manga == null)
        {
            return NotFound();
        }

        manga = _mapper.Map(mangaDTO, manga);

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

    // POST: api/Manga
    [HttpPost]
    public async Task<IActionResult> PostManga
        ([FromForm] MangaDetailDTO mangaDTO, [FromForm] IFormFile? coverImage)
    {
        var manga = _mapper.Map<Manga>(mangaDTO);
        if (coverImage != null)
        {
            manga.CoverPath = mangaDTO.CoverPath =
                await _mediaManager.UploadImage(coverImage, manga.Id);
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
        return CreatedAtAction("GetManga", new { id = mangaDTO.Id }, mangaDTO);
    }

    // DELETE: api/Manga/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteManga(string id)
    {
        if (_context.Mangas == null)
        {
            return NotFound();
        }
        var manga = await _context.Mangas.FindAsync(id);
        if (manga == null)
        {
            return NotFound();
        }

        manga.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MangaExists(string id)
    {
        return (_context.Mangas?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
