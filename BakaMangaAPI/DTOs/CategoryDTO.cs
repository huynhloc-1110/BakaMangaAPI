using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.DTOs;

public class CategoryDTO
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
	public DateTime? DeletedAt { get; set; }
}

public class CategoryEditDTO
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
}
