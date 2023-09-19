using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Request : BaseModel
{
    public ApplicationUser User { get; set; } = default!;
}

public class PromotionRequest : Request
{
    [MaxLength(2000)]
    public string Reason { get; set; } = default!;
    public string EvidenceLink { get; set; } = default!;
}
