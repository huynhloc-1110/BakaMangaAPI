using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Data;
using BakaMangaAPI.Models;
using AutoMapper;
using BakaMangaAPI.DTOs;

namespace BakaMangaAPI.Controllers;

[Route("manage/author")]
[ApiController]
//[Authorize(Roles = "Admin")]
public class ManageAuthorController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ManageAuthorController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Author
    [HttpGet]
    public async Task<IActionResult> GetAuthors([FromQuery] ManageFilterDTO filter)
    {
        var query = _context.Authors.AsQueryable();
        if (!filter.IncludeDeleted)
        {
            query = query.Where(a => a.DeletedAt == null);
        }

        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(m => m.Name.ToLower().Contains(filter.Search.ToLower()));
        }

        var authors = await query
            .OrderBy(a => a.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsNoTracking()
            .ToListAsync();

        return Ok(_mapper.Map<List<AuthorBasicDTO>>(authors));
    }

    // GET: api/Author/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthor(string id)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<AuthorDetailDTO>(author));
    }

    // PUT: api/Author/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAuthor(string id, AuthorDetailDTO authorDTO)
    {
        if (id != authorDTO.Id)
        {
            return BadRequest();
        }

        var author = await _context.Authors
            .SingleOrDefaultAsync(a => a.Id == id);
        if (author == null)
        {
            return NotFound();
        }

        author = _mapper.Map(authorDTO, author);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuthorExists(id))
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

    // POST: api/Author
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<IActionResult> PostAuthor(AuthorDetailDTO authorDTO)
    {
        var author = _mapper.Map<Author>(authorDTO);
        _context.Authors.Add(author);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (AuthorExists(author.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetAuthor", new { id = authorDTO.Id }, authorDTO);
    }

    // DELETE: api/Author/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(string id)
    {
        if (_context.Authors == null)
        {
            return NotFound();
        }
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }

        author.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AuthorExists(string id)
    {
        return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
