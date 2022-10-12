namespace Endava.BookSharing.Application.Filters;

public class PaginationFilter
{
    public PaginationFilter()
    {
        PageNumber = 1;
        PageSize = AppConsts.BooksPerPage;
    }

    public PaginationFilter(int pageNumber)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = AppConsts.BooksPerPage;
    }

    public int PageNumber { get; }
    public int PageSize { get; }
}