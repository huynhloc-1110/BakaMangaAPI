using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Data;
using BakaMangaAPI.Models;
using AutoMapper;
using BakaMangaAPI.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BakaMangaAPI.Controllers;

[Route("manage/user")]
[ApiController]
// [Authorize(Roles = "Admin")]
public class ManageUserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public ManageUserController(ApplicationDbContext context, IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers
        ([FromQuery] ManageUserFilterDTO filter)
    {
        var query = _context.ApplicationUsers
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .AsQueryable();
        if (filter.ExcludeDeleted)
        {
            query = query.Where(u => u.DeletedAt == null);
        }
        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query
                .Where(u => u.Name.ToLower().Contains(filter.Search.ToLower())
                    || u.Email.ToLower().Contains(filter.Search.ToLower()));
        }
        query = filter.RoleOption switch
        {
            RoleOption.All => query,
            _ => query.Where(u => u.UserRoles.Select(ur => ur.Role.Name)
                .Contains(filter.RoleOption.ToString()))
        };

        var users = await query
            .OrderBy(u => u.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsNoTracking()
            .ToListAsync();
        if (users.Count == 0)
        {
            return NotFound();
        }

        var userCount = await query.CountAsync();
        var userList = _mapper.Map<List<UserBasicDTO>>(users);
        var paginatedUserList = new PaginatedListDTO<UserBasicDTO>
            (userList, userCount, filter.Page, filter.PageSize);
        return Ok(paginatedUserList);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoles([FromRoute] string id,
        [FromBody] string[] roles)
    {
        var user = _context.ApplicationUsers
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .SingleOrDefault(u => u.Id == id);
        if (user == null)
        {
            return BadRequest();
        }

        user.UserRoles = new();
        foreach (var role in roles)
        {
            user.UserRoles.Add(new()
            {
                // TODO
            });
        }
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
