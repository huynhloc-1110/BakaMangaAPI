using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class MangaList : BaseModel
{
    [MaxLength(250)]
    public string Name { get; set; } = default!;
    public MangaListType Type { get; set; }

    public ApplicationUser Owner { get; set; } = default!;

    public List<MangaListFollower> Followers { get; set; } = new();
    public List<MangaListItem> Items { get; set; } = new();
}

public enum MangaListType
{
    Private, Public
}
