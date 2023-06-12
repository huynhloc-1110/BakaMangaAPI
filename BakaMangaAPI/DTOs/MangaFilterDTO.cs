namespace BakaMangaAPI.DTOs;

public class MangaFilterDTO
{
    public string Search { get; set; } = string.Empty;

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 12;
}

public class MangaAdvancedFilterDTO : MangaFilterDTO
{
    public SortOption SortOption { get; set; }
}

public enum SortOption
{
    LatestChapter, LatestManga, MostViewDaily, MostViewWeekly, MostViewMonthly, MostViewAll
}
