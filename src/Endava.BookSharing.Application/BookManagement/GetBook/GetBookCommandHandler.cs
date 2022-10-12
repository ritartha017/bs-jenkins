using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models.Options;
using Endava.BookSharing.Application.Models.Response;
using MediatR;
using Microsoft.Extensions.Options;

namespace Endava.BookSharing.Application.BookManagement.GetBook;

public class GetBookCommandHandler : IRequestHandler<GetBookCommand, BookResponse>
{
    private readonly IBookRepository bookRepository;
    private readonly IReviewRepository reviewRepository;
    private readonly IModelMapper _mapper;
    private readonly BookCoverOptions _bookCoverOptions;

    public GetBookCommandHandler(IBookRepository bookRepository,
        IReviewRepository reviewRepository,
        IModelMapper mapper,
        IOptions<BookCoverOptions> bookCoverOptions)
    {
        this.bookRepository = bookRepository;
        this.reviewRepository = reviewRepository;
        _mapper = mapper;
        _bookCoverOptions = bookCoverOptions.Value;
    }

    public async Task<BookResponse> Handle(GetBookCommand request, CancellationToken cancellationToken)
    {
        var bookDetailsDto = await bookRepository.GetByIdDetailsAsync(request.BookId, cancellationToken);
        if (bookDetailsDto is null)
            throw new BookSharingGenericException("Invalid Book ID");

        var isOwner = bookDetailsDto.UploadedBy.Id == request.AuthenticatedUser.Id;
        var isAdmin = request.AuthenticatedUser.IsAdminOrSuperAdmin;

        bookDetailsDto.IsEditable = isOwner || isAdmin;

        var bookResponse = _mapper.Map<BookResponse>(bookDetailsDto);
        bookResponse.Cover = string.Format(_bookCoverOptions.Url, request.BookId);

        var rating = await reviewRepository.GetAverageReviewRating(request.BookId, cancellationToken);
        bookResponse.Rating = Math.Round(rating, 1);

        return bookResponse;
    }

}