using System.ComponentModel.DataAnnotations;

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

public class MangaRequest : Request
{
    [MaxLength(250)]
    public string MangaTitle { get; set; } = default!;

    [MaxLength(100)]
    public string MangaAuthor { get; set; } = default!;

    public string MangaSource { get; set; } = default!;
}

public class OtherRequest : Request
{
    [MaxLength(100)]
    public string Title { get; set; } = default!;

    public string Content { get; set; } = default!;
}

public enum RequestStatus
{
    Processing, Deny, Approve
}
