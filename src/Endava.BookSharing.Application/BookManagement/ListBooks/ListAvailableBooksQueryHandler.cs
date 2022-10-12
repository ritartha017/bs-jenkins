using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models.Options;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Entities.Pagination;
using MediatR;
using Microsoft.Extensions.Options;

namespace Endava.BookSharing.Application.BookManagement.ListBooks;

public class ListAvailableBooksQueryHandler : IRequestHandler<ListAvailableBooksQuery, PaginationList<PaginationBookItem>>
{
    private readonly IBookRepository bookRepository;
    private readonly IGenreRepository genreRepository;
    private readonly ILanguageRepository languageRepository;
    private readonly BookCoverOptions bookCoverOptions;

    public ListAvailableBooksQueryHandler(IBookRepository bookRepository,
        IGenreRepository genreRepository,
        ILanguageRepository languageRepository,
        IOptions<BookCoverOptions> bookCoverOptions)
    {
        this.bookRepository = bookRepository;
        this.genreRepository = genreRepository;
        this.languageRepository = languageRepository;
        this.bookCoverOptions = bookCoverOptions.Value;
    }

    public async Task<PaginationList<PaginationBookItem>> Handle(ListAvailableBooksQuery request, CancellationToken cancellationToken)
    {
        Language? language = null;
        if (request.Language is not null)
            language = await languageRepository.GetByIdAsync(request.Language, cancellationToken);

        var filters = new FilterBookParams()
        {
            Language = language?.Id,
            Genres = (await genreRepository.GetGenresByIdsAsync(request.Genres, cancellationToken)).Select(x => x.Id).ToList(),
            RatingMin = Math.Min(request.RatingMin, request.RatingMax),
            RatingMax = Math.Max(request.RatingMin, request.RatingMax),
            Author = request.Author?.ToLower(),
            IsRatingSpecified = request.IsRatingSpecified
        };

        var bookItems = await bookRepository.GetForPaginationAsync(filters, cancellationToken);

        int itemCount = bookItems.Count;
        if (itemCount == 0)
        {
            throw new BookSharingNotFoundException("There are no books available with this filters.");
        }

        int totalPages = (int)Math.Ceiling(Convert.ToDouble(itemCount) / Convert.ToDouble(AppConsts.BooksPerPage));

        int page = request.Page;
        if (page <= 0)
        {
            page = 1;
        }
        else if (page > totalPages)
        {
            page = totalPages;
        }

        int skipNoOfItems = (page - 1) * AppConsts.BooksPerPage;

        bookItems = bookItems.Skip(skipNoOfItems).Take(AppConsts.BooksPerPage).ToList();

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
