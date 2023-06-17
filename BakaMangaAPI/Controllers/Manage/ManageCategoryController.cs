using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Data;
using BakaMangaAPI.Models;
using AutoMapper;
using BakaMangaAPI.DTOs;

namespace BakaMangaAPI.Controllers;

[Route("manage/category")]
[ApiController]
//[Authorize(Roles = "Admin")]
public class ManageCategoryController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ManageCategoryController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/ManageCategory
    [HttpGet]
	[HttpGet]
	public async Task<IActionResult> GetCategories([FromQuery] ManageFilterDTO filter)
	{
		var query = _context.Categories.AsQueryable();
		if (!filter.IncludeDeleted)
		{
			query = query.Where(a => a.DeletedAt == null);
		}

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(m => m.Name.ToLower().Contains(filter.Search.ToLower()));
		}

		// Get the total count of categories matching the filter criteria
		int totalCategories = await query.CountAsync();

		var categories = await query
			.OrderBy(a => a.Name)
			.Skip((filter.Page - 1) * filter.PageSize)
			.Take(filter.PageSize)
			.AsNoTracking()
			.ToListAsync();

		var response = new
		{
			TotalCount = totalCategories,
			Categories = _mapper.Map<List<CategoryBasicDTO>>(categories)
		};

		return Ok(response);
	}


	// GET: api/ManageCategory/5
	[HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(string id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CategoryDetailDTO>(category));
    }

    // PUT: api/ManageCategory/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(string id, CategoryDetailDTO categoryDTO)
    {
        if (id != categoryDTO.Id)
        {
            return BadRequest();
        }

        var category = await _context.Categories
            .SingleOrDefaultAsync(a => a.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        category = _mapper.Map(categoryDTO, category);

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

    // POST: api/ManageCategory
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<IActionResult> PostCategory(CategoryDetailDTO categoryDTO)
    {
        var category = _mapper.Map<Category>(categoryDTO);
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

        return CreatedAtAction("GetCategory", new { id = categoryDTO.Id }, categoryDTO);
    }

    // DELETE: api/ManageCategory/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(string id)
    {
        if (_context.Categories == null)
        {
            return NotFound();
        }
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        category.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoryExists(string id)
    {
        return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
