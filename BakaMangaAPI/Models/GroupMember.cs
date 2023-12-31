﻿using Microsoft.EntityFrameworkCore;

namespace BakaMangaAPI.Models;

[Index(nameof(GroupId), nameof(UserId), IsUnique = true)]
public class GroupMember
{
    public string GroupId { get; set; } = default!;
    public Group Group { get; set; } = default!;

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public GroupRole GroupRoles { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}

[Flags]
public enum GroupRole
{
    Member = 0, GroupUploader = 1, Moderator = 2, Owner = 4
}
