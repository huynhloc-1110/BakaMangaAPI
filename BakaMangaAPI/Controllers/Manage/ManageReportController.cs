using AutoMapper;
using AutoMapper.QueryableExtensions;

using BakaMangaAPI.Data;
using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Controllers.Manage;

[ApiController]
[Authorize]
[Route("reports")]
public class ManageReportController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public ManageReportController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetReports([FromQuery] ReportFilterDTO filter)
    {
        var query = _context.Reports.OfType<UserReport>();

        if (filter.Status != null)
        {
            query = query.Where(r => r.Status == filter.Status);
        }

        var reportCount = await query.CountAsync();
        var reports = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<UserReportDTO>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        var paginatedUserList = new PaginatedListDTO<UserReportDTO>
            (reports, reportCount, filter.Page, filter.PageSize);
        return Ok(paginatedUserList);
    }

    [HttpPost]
    public async Task<IActionResult> PostReport([FromForm] UserReportEditDTO dto)
    {
        var report = _mapper.Map<UserReport>(dto);
        report.Reporter = await _userManager.GetUserAsync(User);
        report.Reportee = await _userManager.FindByIdAsync(dto.ReporteeId);

        _context.Reports.Add(report);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("{reportId}/status")]
    public async Task<IActionResult> ChangeReportStatus(string reportId, [FromForm] ReportStatus status)
    {
        var report = await _context.Reports.FindAsync(reportId);
        if (report == null)
        {
            return NotFound("Report not found");
        }

        report.Status = status;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
