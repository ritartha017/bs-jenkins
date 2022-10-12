using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Models.Options;
using Endava.BookSharing.Domain.Entities.Pagination;
using MediatR;
using Microsoft.Extensions.Options;

namespace Endava.BookSharing.Application.WishlistManagement.GetBooks;

public class GetBooksFromWishlistQueryHandler : IRequestHandler<GetBooksFromWishlistQuery, PaginationList<PaginationBookItem>>
{
    private readonly IWishlistRepository wishlistRepository;
    private readonly BookCoverOptions bookCoverOptions;

    public GetBooksFromWishlistQueryHandler(IWishlistRepository wishlistRepository, IOptions<BookCoverOptions> bookCoverOptions)
    {
        this.wishlistRepository = wishlistRepository ?? throw new ArgumentNullException(nameof(wishlistRepository));
        this.bookCoverOptions = bookCoverOptions.Value ?? throw new ArgumentNullException(nameof(bookCoverOptions));
    }

    public async Task<PaginationList<PaginationBookItem>> Handle(GetBooksFromWishlistQuery query, CancellationToken cancellationToken)
    {
        int page = query.Page <= 0 ? 1 : query.Page;
        var books = await wishlistRepository.GetBooksByUserIdAsync(query.UserId, cancellationToken);
        int totalPages = (int)Math.Ceiling(Convert.ToDouble(books.Count()) / Convert.ToDouble(AppConsts.BooksPerPage));

        if (page > totalPages) page = totalPages;

        int skipNoOfItems = (page - 1) * AppConsts.BooksPerPage;

        var bookItems = await wishlistRepository.GetForPaginationBooksAsync(query.UserId, skipNoOfItems,
            AppConsts.BooksPerPage, cancellationToken);

        var paginationItems = new List<PaginationBookItem>();

        foreach (var book in bookItems)
        {
            paginationItems.Add(new PaginationBookItem()
            {
                Id = book.Id,
                Cover = string.Format(bookCoverOptions.Url, book.Id),
                Title = book.Title
            });
        }
        var pagination = new PaginationList<PaginationBookItem>()
        {
            Page = page,
            PerPage = AppConsts.BooksPerPage,
            TotalPages = totalPages,
            Items = paginationItems
        };
        return pagination;
    }
}