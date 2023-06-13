namespace BakaMangaAPI.DTOs;

public class FilterDTO
{
    public string Search { get; set; } = string.Empty;

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 12;
}

public class ManageFilterDTO : FilterDTO
{
    public bool IncludeDeleted { get; set; }
}

public class MangaFilterDTO : FilterDTO
{
    public SortOption SortOption { get; set; }
}

public enum SortOption
{
    LatestChapter, LatestManga, MostViewDaily, MostViewWeekly, MostViewMonthly, MostViewAll
}
