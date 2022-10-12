using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Models.Options;
using Endava.BookSharing.Domain.Entities.Pagination;
using MediatR;
using Microsoft.Extensions.Options;

namespace Endava.BookSharing.Application.BookManagement.ListBooksByOwner;

public class ListBooksByOwnerQueryHandler : IRequestHandler<ListBooksByOwnerQuery, PaginationList<ListBooksByOwnerItemsResponse>>
{
    private readonly IBookRepository _bookRepository;
    private readonly PaginationList<ListBooksByOwnerItemsResponse> _paginationList;
    private readonly Helper _helper;

    public ListBooksByOwnerQueryHandler(IBookRepository bookRepository, IOptions<BookCoverOptions> bookCoverOptions)
    {
        _bookRepository = bookRepository;
        _helper = new Helper(bookCoverOptions.Value);
        _paginationList = new();
    }

    public async Task<PaginationList<ListBooksByOwnerItemsResponse>> Handle(ListBooksByOwnerQuery request, CancellationToken cancellationToken)
    {
        var countOfBooksById = await _bookRepository.GetCountByOwnerIdAsync(request.OwnerId, cancellationToken);
        var bookItems = await _bookRepository.GetByOwnerIdAsync(request.OwnerId, request.Filter,
            cancellationToken);
        var bookItemResponses = _helper.GetListBooksItemsResponse(bookItems);

        _paginationList.Page = request.Filter.PageNumber;
            _paginationList.Items = bookItemResponses;
        _paginationList.PerPage = AppConsts.BooksPerPage;
        _paginationList.TotalPages = (int) Math.Ceiling(Convert.ToDouble(countOfBooksById)
                                                        / Convert.ToDouble(AppConsts.BooksPerPage));

        return _paginationList;
    }
}