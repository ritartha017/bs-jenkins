namespace Endava.BookSharing.Domain.Entities.Pagination;

public class PaginationList<T>
{
    public int Page { get; set; } = 1;
    public int PerPage { get; set; }
    public int TotalPages { get; set; }
    public ICollection<T> Items { get; set; } = new List<T>();
}

