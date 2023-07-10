using BakaMangaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.DTOs;

public class ChapterBasicDTO
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public Language Language { get; set; }

    public UserSimpleDTO Uploader { get; set; } = default!;

    public int ViewCount { get; set; }
}
