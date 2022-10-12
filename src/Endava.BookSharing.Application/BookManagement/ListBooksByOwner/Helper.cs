using System.Globalization;
using Endava.BookSharing.Application.Filters;
using Endava.BookSharing.Application.Models.Options;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Entities.Pagination;

namespace Endava.BookSharing.Application.BookManagement.ListBooksByOwner;

public class Helper
{
    private readonly ICollection<ListBooksByOwnerItemsResponse> _itemsResponses;
    private readonly BookCoverOptions _bookCoverOptions;

    public Helper(BookCoverOptions bookCoverOptions)
    {
        _itemsResponses = new List<ListBooksByOwnerItemsResponse>();
        _bookCoverOptions = bookCoverOptions;
    }

    public ICollection<ListBooksByOwnerItemsResponse> GetListBooksItemsResponse(ICollection<Book> books)
    {
        foreach (var book in books)
        {
            _itemsResponses.Add(new ListBooksByOwnerItemsResponse
            {
                Id = book.Id,
                Cover = string.Format(CultureInfo.CurrentCulture, _bookCoverOptions.Url, book.Id),
                Title = book.Title
            });
        }

        return _itemsResponses;
    }
}