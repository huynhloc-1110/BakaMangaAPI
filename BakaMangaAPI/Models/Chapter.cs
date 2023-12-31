﻿using System.ComponentModel.DataAnnotations;

namespace BakaMangaAPI.Models;

public class Chapter : BaseModel
{
    [Range(0, float.MaxValue)]
    public float Number { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = default!;

    public Language Language { get; set; }

    public Manga Manga { get; set; } = default!;
    public ApplicationUser Uploader { get; set; } = default!;
    public Group? UploadingGroup { get; set; }

    public List<ChapterImage> Images { get; set; } = new();
    public List<ChapterComment> Comments { get; set; } = new();
    public List<ChapterView> ChapterViews { get; set; } = new();
}
