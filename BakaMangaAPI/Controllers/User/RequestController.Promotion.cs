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
    [HttpGet("~/promotion-requests")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> GetPromotionRequests([FromQuery] FilterDTO filter)
    {
        var requestQuery = _context.Requests
            .OfType<PromotionRequest>()
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
            .ProjectTo<PromotionRequestDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        var paginatedList = new PaginatedListDTO<PromotionRequestDTO>
            (requests, requestCount, filter.Page, filter.PageSize);
        return Ok(paginatedList);
    }

    [HttpGet("~/users/me/promotion-requests")]
    public async Task<IActionResult> GetMyPromotionRequests([FromQuery] DateTime? createdAtCursor)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var requests = await _context.Requests
            .OfType<PromotionRequest>()
            .Where(r => r.User.Id == currentUserId)
            .Where(r => createdAtCursor == null || r.CreatedAt < createdAtCursor)
            .OrderByDescending(r => r.CreatedAt)
            .ProjectTo<PromotionRequestDTO>(_mapper.ConfigurationProvider)
            .Take(4)
            .ToListAsync();

        return Ok(requests);
    }

    [HttpPost("~/promotion-requests")]
    public async Task<IActionResult> PostPromotionRequest([FromForm] PromotionRequestEditDTO dto)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var isAlreadyUploader = await _userManager.IsInRoleAsync(currentUser, "Uploader");
        if (isAlreadyUploader)
        {
            return BadRequest("The current user is already uploader");
        }

        var isProcessing = await _context.Requests
            .OfType<PromotionRequest>()
            .AnyAsync(r => r.User == currentUser && r.Status == RequestStatus.Processing);
        if (isProcessing)
        {
            return BadRequest("The user request is processed. Can't send any more requests");
        }

        var request = _mapper.Map<PromotionRequest>(dto);
        request.User = currentUser;
        _context.Requests.Add(request);

        await _context.SaveChangesAsync();
        return Ok(_mapper.Map<PromotionRequestDTO>(request));
    }

    [HttpPut("~/promotion-requests/{requestId}/status")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<ActionResult> ChangePromotionRequestStatus(string requestId,
        [FromForm] RequestStatus status)
    {
        var request = await _context.Requests
            .OfType<PromotionRequest>()
            .Include(r => r.User)
            .SingleOrDefaultAsync(r => r.Id == requestId);
        if (request == null)
        {
            return NotFound("Request not found");
        }

        if (request.Status != RequestStatus.Processing)
        {
            return Forbid();
        }

        request.Status = status;
        if (status == RequestStatus.Approve)
        {
            await _userManager.AddToRoleAsync(request.User, "Uploader");
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
