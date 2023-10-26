using BakaMangaAPI.Models;

namespace BakaMangaAPI.DTOs;

public class FilterDTO
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public bool IncludeDeleted { get; set; }
}

public class UserFilterDTO : FilterDTO
{
    public RoleOption RoleOption { get; set; }
}

public class MemberFilterDTO : FilterDTO
{
    public GroupRole GroupRoleOptions { get; set; }
}

public class MangaFilterDTO : FilterDTO
{
    public SortOption SortOption { get; set; }
    public List<string> IncludedCategoryIds { get; set; } = new();
    public List<string> ExcludedCategoryIds { get; set; } = new();
    public List<Language> SelectedLanguages { get; set; } = new();
    public string? SelectedAuthorId { get; set; }
}

public class ReportFilterDTO : FilterDTO
{
    public ReportStatus? Status { get; set; }
}

public enum RoleOption
{
    All, User, Uploader, Manager, Admin
}

public enum SortOption
{
    Default,
    LatestChapter, LatestManga,
    MostViewDaily, MostView,
    MostFollow, BestRating,
}
