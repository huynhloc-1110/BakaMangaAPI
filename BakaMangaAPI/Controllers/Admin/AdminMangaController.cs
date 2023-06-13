using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services;
using AutoMapper;

namespace BakaMangaAPI.Controllers;

[Route("admin/manga")]
[ApiController]
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

    // PUT: api/Manga/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Manga>> PostManga
        ([FromForm] MangaDetailDTO mangaDTO, [FromForm] IFormFile? coverImage)
    {
        var manga = _mapper.Map<Manga>(mangaDTO);
        if (coverImage != null)
        {
            manga.CoverPath = mangaDTO.CoverPath =
                await _mediaManager.UploadImage(coverImage, manga.Id);
            await _context.SaveChangesAsync();
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
        return CreatedAtAction("GetManga", "Manga", new { id = manga.Id }, mangaDTO);
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
