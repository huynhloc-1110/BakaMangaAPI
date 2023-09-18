using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.DTOs;

public class AuthorDTO
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Biography { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}

public class AuthorEditDTO
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Biography { get; set; } = string.Empty;
}
