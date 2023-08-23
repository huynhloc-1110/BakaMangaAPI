using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Data;
using BakaMangaAPI.Models;
using AutoMapper;
using BakaMangaAPI.DTOs;

namespace BakaMangaAPI.Controllers;

[Route("manage/category")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ManageCategoryController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ManageCategoryController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: manage/category?Search=&Page=1&PageSize=12
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategories([FromQuery] FilterDTO filter)
    {
        var query = _context.Categories.AsQueryable();
        if (filter.ExcludeDeleted)
        {
            query = query.Where(a => a.DeletedAt == null);
        }
        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(m => m.Name.ToLower().Contains(filter.Search.ToLower()));
        }

        var categories = await query
            .OrderBy(a => a.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsNoTracking()
            .ToListAsync();
        if (categories.Count == 0)
        {
            return NotFound();
        }

        var categoryCount = await query.CountAsync();
        var categoryList = _mapper.Map<List<CategoryDTO>>(categories);
        var paginatedCategoryList = new PaginatedListDTO<CategoryDTO>
            (categoryList, categoryCount, filter.Page, filter.PageSize);
        return Ok(paginatedCategoryList);
    }

    // GET: manage/category/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(string id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CategoryDTO>(category));
    }

    // PUT: manage/category/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(string id, CategoryEditDTO categoryEditDTO)
    {
        if (id != categoryEditDTO.Id)
        {
            return BadRequest();
        }

        var category = await _context.Categories
            .SingleOrDefaultAsync(a => a.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        category = _mapper.Map(categoryEditDTO, category);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
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

    // POST: manage/category
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<IActionResult> PostCategory(CategoryEditDTO categoryEditDTO)
    {
        var category = _mapper.Map<Category>(categoryEditDTO);
        _context.Categories.Add(category);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (CategoryExists(category.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        var categoryDTO = _mapper.Map<CategoryDTO>(category);
        return CreatedAtAction("GetCategory", new { id = categoryDTO.Id }, categoryDTO);
    }

    // DELETE: manage/category/5?undelete=false
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(string id, [FromQuery] bool undelete)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        if (undelete)
        {
            category.DeletedAt = null;
        }
        else
        {
            category.DeletedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool CategoryExists(string id)
    {
        return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
