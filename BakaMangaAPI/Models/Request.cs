using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace BakaMangaAPI.Models;

public class Request : BaseModel
{
    public ApplicationUser User { get; set; } = default!;
    public RequestStatus Status { get; set; }
    public bool StatusConfirmed { get; set; } // for notify user
}

public class PromotionRequest : Request
{
    [MaxLength(2000)]
    public string Reason { get; set; } = default!;
    public string EvidenceLink { get; set; } = default!;
}

public class JoinGroupRequest : Request
{
    public Group Group { get; set; } = default!;
}

public enum RequestStatus
{
    Processing, Ignore, Deny, Approve
}
