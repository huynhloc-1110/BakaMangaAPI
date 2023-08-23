namespace BakaMangaAPI.DTOs;

public class FilterDTO
{
    public string? Search { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 12;

    public bool ExcludeDeleted { get; set; } = true;
}

public class UserFilterDTO : FilterDTO
{
    public RoleOption RoleOption { get; set; }
}

public class MangaFilterDTO : FilterDTO
{
    public SortOption SortOption { get; set; }

    public string? IncludedCategoryIds { get; set; }
    public string? ExcludedCategoryIds { get; set; }
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
