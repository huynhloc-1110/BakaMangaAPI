using AutoMapper;
using AutoMapper.QueryableExtensions;

using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers;

[Route("manage/categories")]
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

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategories([FromQuery] FilterDTO filter)
    {
        var query = _context.Categories.AsQueryable();
        
        if (filter.IncludeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }
        
        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(m => m.Name.ToLower().Contains(filter.Search.ToLower()));
        }

        var categoryCount = await query.CountAsync();
        var categories = await query
            .OrderBy(a => a.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsNoTracking()
            .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var paginatedCategoryList = new PaginatedListDTO<CategoryDTO>(
            categories, categoryCount, filter.Page, filter.PageSize);
        return Ok(paginatedCategoryList);
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetCategory(string categoryId)
    {
        var category = await _context.Categories
            .IgnoreQueryFilters()
            .Where(c => c.Id == categoryId)
            .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

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
        return CreatedAtAction(nameof(GetCategory), new { categoryId = categoryDTO.Id }, categoryDTO);
    }

    [HttpPut("{categoryId}")]
    public async Task<IActionResult> PutCategory(string categoryId, CategoryEditDTO categoryEditDTO)
    {
        var category = await _context.Categories
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(a => a.Id == categoryId);

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
            if (!CategoryExists(categoryId))
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

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteCategory(string categoryId, [FromQuery] bool undelete)
    {
        var category = await _context.Categories
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(c => c.Id == categoryId);

        if (category == null)
        {
            return NotFound();
        }

        category.DeletedAt = undelete ? null : DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool CategoryExists(string id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }
}
