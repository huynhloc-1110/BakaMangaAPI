using BakaMangaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.DTOs;

public class CategoryBasicDTO
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;
}

public class CategoryDetailDTO
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public DateTime? DeletedAt { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
}
