using System.ComponentModel.DataAnnotations;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.DTOs;

public class UserReportEditDTO
{
    [MaxLength(2000)]
    public string Reason { get; set; } = default!;
    public string ReporteeId { get; set; } = default!;
}

public class UserReportDTO
{
    public string Id { get; set; } = default!;
    public UserSimpleDTO Reporter { get; set; } = default!;
    public UserSimpleDTO Reportee { get; set; } = default!;
    public string Reason { get; set; } = default!;
    public ReportStatus Status { get; set; }
}
