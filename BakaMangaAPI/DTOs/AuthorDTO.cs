using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.DTOs;

public class AuthorBasicDTO
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Biography { get; set; } = default!;
}

public class AuthorDetailDTO
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public DateTime? DeletedAt { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Biography { get; set; } = string.Empty;
}
