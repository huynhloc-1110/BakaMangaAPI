using AutoMapper;
using AutoMapper.QueryableExtensions;

using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Manage;

[Route("manage/authors")]
[ApiController]
[Authorize(Roles = "Manager,Admin")]
public class ManageAuthorController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ManageAuthorController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAuthors([FromQuery] FilterDTO filter)
    {
        var query = _context.Authors.AsQueryable();

        if (filter.IncludeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(m => m.Name.ToLower().Contains(filter.Search.ToLower()));
        }

        var authorCount = await query.CountAsync();
        var authors = await query
            .OrderBy(a => a.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<AuthorDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        var paginatedAuthorList = new PaginatedListDTO<AuthorDTO>
            (authors, authorCount, filter.Page, filter.PageSize);
        return Ok(paginatedAuthorList);
    }

    [HttpGet("{authorId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAuthor(string authorId)
    {
        var author = await _context.Authors
            .IgnoreQueryFilters()
            .Where(a => a.Id == authorId)
            .ProjectTo<AuthorDTO>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

        if (author == null)
        {
            return NotFound();
        }

        return Ok(author);
    }

    [HttpPost]
    public async Task<IActionResult> PostAuthor(AuthorEditDTO authorEditDTO)
    {
        var author = _mapper.Map<Author>(authorEditDTO);
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

        var authorDTO = _mapper.Map<AuthorDTO>(author);
        return CreatedAtAction(nameof(GetAuthor), new { authorId = authorDTO.Id }, authorDTO);
    }

    [HttpPut("{authorId}")]
    public async Task<IActionResult> PutAuthor(string authorId, AuthorEditDTO authorEditDTO)
    {
        var author = await _context.Authors
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(a => a.Id == authorId);

        if (author == null)
        {
            return NotFound();
        }

        author = _mapper.Map(authorEditDTO, author);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuthorExists(authorId))
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

    [HttpDelete("{authorId}")]
    public async Task<IActionResult> DeleteAuthor(string authorId, [FromQuery] bool undelete)
    {
        var author = await _context.Authors
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(a => a.Id == authorId);

        if (author == null)
        {
            return NotFound();
        }

        author.DeletedAt = undelete ? null : DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool AuthorExists(string id)
    {
        return _context.Authors.Any(e => e.Id == id);
    }
}
