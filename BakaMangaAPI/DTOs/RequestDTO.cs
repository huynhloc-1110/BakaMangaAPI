using System.ComponentModel.DataAnnotations;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.DTOs;

public class RequestDTO
{
    public string Id { get; set; } = default!;
    public UserSimpleDTO User { get; set; } = default!;
    public RequestStatus Status { get; set; }
    public bool StatusConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class JoinGroupRequestDTO : RequestDTO
{
    public GroupBasicDTO Group { get; set; } = default!;
}

public class PromotionRequestDTO : RequestDTO
{
    public string Reason { get; set; } = default!;
    public string EvidenceLink { get; set; } = default!;
}

public class MangaRequestDTO : RequestDTO
{
    public string MangaTitle { get; set; } = default!;
    public string MangaAuthor { get; set; } = default!;
    public string MangaSource { get; set; } = default!;
}

public class OtherRequestDTO : RequestDTO
{
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
}

public class PromotionRequestEditDTO
{
    [MaxLength(2000)]
    public string Reason { get; set; } = default!;

    public string EvidenceLink { get; set; } = default!;
}

public class MangaRequestEditDTO
{
    [MaxLength(250)]
    public string MangaTitle { get; set; } = default!;

    [MaxLength(100)]
    public string MangaAuthor { get; set; } = default!;

    public string MangaSource { get; set; } = default!;
}

public class OtherRequestEditDTO
{
    [MaxLength(100)]
    public string Title { get; set; } = default!;

    public string Content { get; set; } = default!;
}
