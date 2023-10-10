namespace BakaMangaAPI.DTOs;

public class MangaBlockDTO : MangaSimpleDTO
{
    public List<ChapterBasicDTO> Chapters { get; set; } = new();

    public DateTime UpdatedAt { get; set; }
}

public class UploaderMangaBlockDTO : MangaBlockDTO { }
public class GroupMangaBlockDTO : MangaBlockDTO { }