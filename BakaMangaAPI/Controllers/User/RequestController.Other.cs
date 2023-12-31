using System.Security.Claims;

using AutoMapper.QueryableExtensions;

using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.User;

public partial class RequestController
{
    [HttpGet("~/other-requests")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> GetOtherRequests([FromQuery] FilterDTO filter)
    {
        var requestQuery = _context.Requests
            .OfType<OtherRequest>()
            .Where(r => r.Status == RequestStatus.Processing);

        if (!string.IsNullOrEmpty(filter.Search))
        {
            requestQuery = requestQuery.Where(r => r.User.Name.ToLower()
                .Contains(filter.Search.ToLower()));
        }

        var requestCount = await requestQuery.CountAsync();
        var requests = await requestQuery
            .OrderByDescending(r => r.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<OtherRequestDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        var paginatedList = new PaginatedListDTO<OtherRequestDTO>
            (requests, requestCount, filter.Page, filter.PageSize);
        return Ok(paginatedList);
    }

    [HttpGet("~/users/me/other-requests")]
    public async Task<IActionResult> GetMyOtherRequests([FromQuery] DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var requests = await _context.Requests
            .OfType<OtherRequest>()
            .Where(r => r.User.Id == currentUserId)
            .Where(r => createdAtCursor == null || r.CreatedAt < createdAtCursor)
            .OrderByDescending(r => r.CreatedAt)
            .ProjectTo<OtherRequestDTO>(_mapper.ConfigurationProvider)
            .Take(4)
            .ToListAsync();

        return Ok(requests);
    }

    [HttpPost("~/other-requests")]
    public async Task<IActionResult> PostOtherRequest([FromForm] OtherRequestEditDTO dto)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        var request = _mapper.Map<OtherRequest>(dto);
        request.User = currentUser;
        _context.Requests.Add(request);

        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<OtherRequestDTO>(request));
    }
}
