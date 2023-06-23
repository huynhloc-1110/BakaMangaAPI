using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Data;
using BakaMangaAPI.Models;
using AutoMapper;
using BakaMangaAPI.DTOs;

namespace BakaMangaAPI.Controllers;

[Route("manage/user")]
[ApiController]
// [Authorize(Roles = "Admin")]
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
    public async Task<IActionResult> GetUsers
        ([FromQuery] ManageUserFilterDTO filter)
    {
        var query = filter.RoleOption switch
        {
            RoleOption.All => _context.ApplicationUsers.AsQueryable(),
            _ => GetUsersByRoleQuery(filter.RoleOption.ToString())
        };
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

    private IQueryable<ApplicationUser> GetUsersByRoleQuery(string roleName)
    {
        return from userrole in _context.UserRoles
               join user in _context.ApplicationUsers
                   on userrole.UserId equals user.Id
               join role in _context.Roles
                   on userrole.RoleId equals role.Id
               where role.Name.Equals(roleName)
               select user;
    }
}
