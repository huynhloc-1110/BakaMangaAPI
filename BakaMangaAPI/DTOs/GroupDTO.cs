﻿namespace BakaMangaAPI.DTOs;

public class GroupBasicDTO
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public int MemberNumber { get; set; }

    public string? AvatarPath { get; set; }
}
