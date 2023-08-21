namespace BakaMangaAPI.DTOs;

public class FilterDTO
{
    public string? Search { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 12;
}

public class ManageFilterDTO : FilterDTO
{
    public bool ExcludeDeleted { get; set; }
}

public class ManageUserFilterDTO : ManageFilterDTO
{
    public RoleOption RoleOption { get; set; }
}

public enum RoleOption
{
    All, User, Uploader, Manager, Admin
}

public class MangaFilterDTO : FilterDTO
{
    public SortOption SortOption { get; set; }
}

public enum SortOption
{
    Default,
    LatestChapter, LatestManga,
    MostViewDaily, MostView,
    MostFollow, BestRating,
}
