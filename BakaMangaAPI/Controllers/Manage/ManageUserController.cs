using AutoMapper;
using AutoMapper.QueryableExtensions;

using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Manage;

[Route("manage/users")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ManageUserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ManageUserController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilterDTO filter)
    {
        var query = _context.ApplicationUsers.AsQueryable();

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

        var userCount = await query.CountAsync();
        var users = await query
            .OrderBy(u => u.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<UserBasicDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        var paginatedUserList = new PaginatedListDTO<UserBasicDTO>
            (users, userCount, filter.Page, filter.PageSize);
        return Ok(paginatedUserList);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateRoles([FromRoute] string userId, [FromBody] string[] roles)
    {
        var user = _context.ApplicationUsers
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return NotFound("User not found");
        }

        user.UserRoles = new();
        foreach (var role in roles)
        {
            user.UserRoles.Add(new()
            {
                Role = _context.ApplicationRoles.SingleOrDefault(r => r.Name == role)!
            });
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{userId}/restore")]
    public async Task<IActionResult> RestoreUser(string userId)
    {
        var user = await _context.ApplicationUsers.FindAsync(userId);

        if (user == null)
        {
            return NotFound("User does not exist");
        }
        if (user.DeletedAt == null)
        {
            return BadRequest("User is not deleted");
        }

        user.DeletedAt = null;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{userId}/ban")]
    public async Task<IActionResult> BanUser(string userId, [FromForm] int bannedDayNum)
    {
        var user = await _context.ApplicationUsers.FindAsync(userId);
        if (user == null)
        {
            return NotFound("User does not exist");
        }

        user.BannedUntil = DateTime.UtcNow.AddDays(bannedDayNum);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{userId}/unban")]
    public async Task<IActionResult> UnbanUser(string userId)
    {
        var user = await _context.ApplicationUsers.FindAsync(userId);
        if (user == null)
        {
            return NotFound("User does not exist");
        }

        user.BannedUntil = null;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
