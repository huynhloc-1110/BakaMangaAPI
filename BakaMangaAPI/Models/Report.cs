using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Report : BaseModel
{
    [MaxLength(2000)]
    public string Reason { get; set; } = default!;
    public ReportStatus Status { get; set; }

    public ApplicationUser Reporter { get; set; } = default!;
}

public enum ReportStatus
{
    Pending, Resolved, Omitted
}

public class UserReport : Report
{
    public ApplicationUser Reportee { get; set; } = default!;
}
