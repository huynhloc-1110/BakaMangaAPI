﻿using BakaMangaAPI.Models;

namespace BakaMangaAPI.DTOs;

public class MangaListBasicDTO
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public MangaListType Type { get; set; }
    public UserSimpleDTO Owner { get; set; } = default!;
    public List<string?> MangaCoverUrls { get; set; } = new();
    public DateTime UpdatedAt { get; set; }
    public bool AlreadyAdded { get; set; }
    public bool AlreadyFollowed { get; set; }
}

public class MangaListEditDTO
{
    public string Name { get; set; } = default!;
    public MangaListType Type { get; set; }
    public string? AddedMangaId { get; set; }
    public string? RemovedMangaId { get; set; }
}
