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
    [HttpGet("~/manga-requests")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> GetMangaRequests([FromQuery] FilterDTO filter)
    {
        var requestQuery = _context.Requests
            .OfType<MangaRequest>()
            .Where(r => r.Status == RequestStatus.Processing);

        if (!string.IsNullOrEmpty(filter.Search))
        {
            requestQuery = requestQuery.Where(r => r.User.Name.ToLower()
                .Contains(filter.Search.ToLower()));
        }

        var requestCount = await requestQuery.CountAsync();
        var requests = await requestQuery
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<MangaRequestDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        var paginatedList = new PaginatedListDTO<MangaRequestDTO>
            (requests, requestCount, filter.Page, filter.PageSize);
        return Ok(paginatedList);
    }

    [HttpGet("~/users/me/manga-requests")]
    public async Task<IActionResult> GetMyMangaRequests([FromQuery] DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var requests = await _context.Requests
            .OfType<MangaRequest>()
            .Where(r => r.User.Id == currentUserId)
            .Where(r => createdAtCursor == null || r.CreatedAt < createdAtCursor)
            .OrderByDescending(r => r.CreatedAt)
            .ProjectTo<MangaRequestDTO>(_mapper.ConfigurationProvider)
            .Take(4)
            .ToListAsync();

        return Ok(requests);
    }

    [HttpPost("~/manga-requests")]
    public async Task<IActionResult> PostMangaRequest([FromForm] MangaRequestEditDTO dto)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        var request = _mapper.Map<MangaRequest>(dto);
        request.User = currentUser;
        _context.Requests.Add(request);

        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<MangaRequestDTO>(request));
    }
}
