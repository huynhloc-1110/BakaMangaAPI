using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using AutoMapper;

namespace BakaMangaAPI.Controllers;

[Route("user/[controller]")]
[ApiController]
public class MangaController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public MangaController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Manga
    [HttpGet("{option=latest-chapter}/{page=1}/{itemPerPage=4}")]
    public async Task<ActionResult<IEnumerable<MangaBasicDTO>>> GetMangas
        (string option, int page, int itemPerPage)
    {
        // validate param
        string[] options = { "latest-chapter", "latest-manga", "popular" };
        if (!options.Contains(option) || page <= 0 || itemPerPage <= 0)
        {
            return BadRequest();
        }

        // construct and call query
        var mangaQuery = _context.Mangas.Where(m => m.DeletedAt == null);
        mangaQuery = option switch
        {
            "latest-chapter" => mangaQuery
                .Include(m => m.Chapters)
                .OrderByDescending(m => m.Chapters.Max(c => c.CreatedAt)),
            "latest-manga" => mangaQuery
                .OrderByDescending(m => m.CreatedAt),
            _ => throw new NotImplementedException()
        };
        var mangas = await mangaQuery
            .Skip((page - 1) * itemPerPage)
            .Take(itemPerPage)
            .AsNoTracking()
            .ToListAsync();

        // map to DTO list
        return _mapper.Map<List<MangaBasicDTO>>(mangas);
    }

    // GET: api/Manga/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MangaDetailDTO>> GetManga(string id)
    {
        var manga = await _context.Mangas
            .Where(m => m.DeletedAt == null)
            .Include(m => m.Chapters)
            .AsNoTracking()
            .AsSplitQuery()
            .SingleOrDefaultAsync(m => m.Id == id);

        if (manga == null || manga.DeletedAt != null)
        {
            return NotFound();
        }

        return _mapper.Map<MangaDetailDTO>(manga);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetMangaCount()
    {
        return await _context.Mangas
            .Where(m => m.DeletedAt == null)
            .CountAsync();
    }
}
