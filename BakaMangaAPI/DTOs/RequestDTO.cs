using BakaMangaAPI.DTOs;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.DTOs;

public class JoinGroupRequestDTO
{
    public string Id { get; set; } = default!;
    public UserSimpleDTO User { get; set; } = default!;
    public GroupBasicDTO Group { get; set; } = default!;
    public RequestStatus Status { get; set; }
    public bool StatusConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
}