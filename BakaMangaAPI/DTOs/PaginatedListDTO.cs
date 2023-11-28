namespace BakaMangaAPI.DTOs;

public class PaginatedListDTO<T>
{
    public List<T> ItemList { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public PaginatedListDTO(List<T> itemList, int totalCount, int page, int pageSize)
    {
        ItemList = itemList;
        CurrentPage = page;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
