using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public abstract class BaseModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [DataType(DataType.DateTime)]
    public DateTime? DeletedAt { get; set; }
}

public abstract class BaseModelWithCreatedAt : BaseModel
{
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
